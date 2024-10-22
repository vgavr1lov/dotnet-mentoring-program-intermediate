using DataLibrary;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DataCaptureService;

internal class Program
{
    private const string DocumentDirectory = @"Documents";
    private const string FileExtension = "*.pdf";
    private const string ProcessedFilePrefix = "archive-";
    private const string DocumentCaptureExchange = "DocumentCaptureExchange";
    private const string CentralizedControlExchange = "CentralizedControlExchange";
    private static int MessageSize
    {
        get { return messageSize; }
        set { messageSize = value * 1024 * 1024; }
    }

    private static int messageSize = 10 * 1024 * 1024;
    private static List<string> consoleDisplayedCentralizedControlNotificationBuffer = new();
    private static List<string> consoleDisplayedReadingProgressBuffer = new();
    private static IConnection? Connection { get; set; }
    private static IModel? DocumentChannel { get; set; }
    private static IModel? ControlChannel { get; set; }
    private static string? Queue { get; set; }
    private static CaptureState CurrentState { get; set; } = CaptureState.WaitingForFiles;

    static void Main(string[] args)
    {
        Task.Run(() =>
        {
            while (true)
            {
                ControlChannel = CreateChannel(ControlChannel);
                CreateExchange(CentralizedControlExchange, ControlChannel);
                ReceiveCommandFromCentrilizedControl();
                SendStatus();
                Task.Delay(3000).Wait();
            }
        });

        while (true)
        {
            WatchForFiles();
            Thread.Sleep(3000);
        }
    }

    private static void ReceiveCommandFromCentrilizedControl()
    {
        if (Queue == null)
            Queue = Guid.NewGuid().ToString();


        ControlChannel?.QueueDeclare(Queue, true, false, false);
        ControlChannel?.QueueBind(Queue, CentralizedControlExchange, "centralized.control");

        var consumer = new EventingBasicConsumer(ControlChannel);
        consumer.Received += (sender, args) =>
        {
            var headers = args.BasicProperties.Headers;
            if (headers != null)
            {
                var messageSize = Convert.ToInt32(headers["NewMessageSize"]);
                MessageSize = messageSize;
                var message = $"{DateTime.Now} New message size is {messageSize}";
                consoleDisplayedCentralizedControlNotificationBuffer.Add(message);
                Console.WriteLine(message);
            }
        };

        ControlChannel?.BasicConsume(Queue, true, consumer);
    }

    private static void SendStatus()
    {
        var headers = new Dictionary<string, object?>
                {
                    { "CurrentState", CurrentState.ToString() },
                    { "MessageSize", MessageSize }
                };
        var basicProperties = ControlChannel?.CreateBasicProperties();
        basicProperties.Headers = headers;
        ControlChannel?.BasicPublish(CentralizedControlExchange, "status.update", basicProperties, null);
    }

    private static void WatchForFiles()
    {
        CurrentState = CaptureState.WaitingForFiles;
        Console.Clear();
        Console.WriteLine("Waiting for files...");
        var files = Directory.GetFiles(DocumentDirectory, FileExtension, SearchOption.TopDirectoryOnly)
            .Where(f => !Path.GetFileName(f)
            .StartsWith(ProcessedFilePrefix))
            .ToArray();

        if (files.Length > 0)
        {
            CurrentState = CaptureState.ProcessingFiles;
            RetrieveDocuments(files);
        }

        CurrentState = CaptureState.WaitingForFiles;
    }

    private static void RetrieveDocuments(string[] files)
    {
        var messageChunks = new Dictionary<string, List<PreparedMessageBytes>>();
        var preparedMessageBytes = new List<PreparedMessageBytes>();
        var numberOfFiles = files.Length;
        var numberOfProcessedFiles = 0;
        var clusterSize = 0;

        for (int i = 0; i < numberOfFiles; i++)
        {
            numberOfProcessedFiles = i + 1;
            var fileInfo = new FileInfo(files[i]);
            var fileLength = fileInfo.Length;
            var fileName = fileInfo.Name;

            clusterSize = CalculateClusterSize(fileLength, MessageSize);
            messageChunks.Add(fileName, new List<PreparedMessageBytes>());

            using (var reader = new FileStream(files[i], FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var bytesRead = 0;
                var currentPosition = 0;


                while (bytesRead < fileLength)
                {
                    var messageSizeChunk = new byte[MessageSize];
                    var readMessageSizeChunk = reader.Read(messageSizeChunk, 0, MessageSize);

                    if (readMessageSizeChunk == 0)
                        break;

                    bytesRead += readMessageSizeChunk;
                    UpdateReadingProgress(numberOfFiles, numberOfProcessedFiles, fileName, bytesRead, fileLength);

                    messageChunks[fileName].Add(new PreparedMessageBytes
                    {
                        DataBytes = messageSizeChunk,
                        ActualDataLength = readMessageSizeChunk,
                        PositionIdentifier = currentPosition,
                        ClusterSize = clusterSize
                    });
                }

                currentPosition++;
            }

            RenameProcessedFile(files[i]);
        }

        Send(messageChunks);
    }

    private static int CalculateClusterSize(long fileLength, int messageSize)
    {
        return (int)Math.Ceiling((double)fileLength / messageSize);
    }

    private static void UpdateReadingProgress(int numberOfFiles, int numberOfProcessedFiles, string fileName, int bytesRead, long totalBytes)
    {
        Console.Clear();
        foreach (var bufferedMessage in consoleDisplayedCentralizedControlNotificationBuffer)
        {
            Console.WriteLine(bufferedMessage);
        }
        consoleDisplayedReadingProgressBuffer.Clear();

        var processedFilesMessage = $"Processed {numberOfProcessedFiles}/{numberOfFiles} files.";
        consoleDisplayedReadingProgressBuffer.Add(processedFilesMessage);
        Console.WriteLine(processedFilesMessage);

        var percentage = (double)bytesRead / totalBytes * 100;
        var readingCompleteMessage = $"Reading {fileName}: {percentage:F2}% complete";
        consoleDisplayedReadingProgressBuffer.Add(readingCompleteMessage);
        Console.WriteLine(readingCompleteMessage);
    }

    private static void UpdateSendingProgress(int totalNumberOfMessages, int sentMessages, int totalNumberOfChunks, int sentChunks)
    {
        Console.Clear();
        foreach (var bufferedMessage in consoleDisplayedCentralizedControlNotificationBuffer)
        {
            Console.WriteLine(bufferedMessage);
        }
        foreach (var bufferedMessage in consoleDisplayedReadingProgressBuffer)
        {
            Console.WriteLine(bufferedMessage);
        }
        Console.WriteLine($"Sent {sentMessages}/{totalNumberOfMessages} messages.");
        Console.WriteLine($"Sent {sentChunks}/{totalNumberOfChunks} message chunks.");
    }
    private static void RenameProcessedFile(string file)
    {
        var newFilename = Path.Combine(DocumentDirectory, ProcessedFilePrefix + Path.GetFileName(file));
        Task.Run(() =>
        {
            File.Move(file, newFilename);
        });
    }
    private static void Send(Dictionary<string, List<PreparedMessageBytes>> messageChunks)
    {
        DocumentChannel = CreateChannel(DocumentChannel);
        CreateExchange(DocumentCaptureExchange, DocumentChannel);
        SendMessages(messageChunks);
    }

    private static void SendMessages(Dictionary<string, List<PreparedMessageBytes>> messageChunks)
    {
        var totalNumberOfMessaes = messageChunks.Count;
        var sentMessages = 0;

        foreach (var messageChunk in messageChunks)
        {
            var totalNumberOfMessageChunks = messageChunk.Value.Count;
            var numberOfProcessedMessageChunks = 0;
            sentMessages++;

            for (int i = 0; i < totalNumberOfMessageChunks; i++)
            {
                numberOfProcessedMessageChunks = i + 1;
                UpdateSendingProgress(totalNumberOfMessaes, sentMessages, totalNumberOfMessageChunks, numberOfProcessedMessageChunks);
                var bytes = messageChunk.Value[i].DataBytes;
                var actualDataLength = messageChunk.Value[i].ActualDataLength;

                var headers = new Dictionary<string, object?>
                {
                    { "SequenceIdentifier", messageChunk.Key },
                    { "PositionIdentifier", messageChunk.Value[i].PositionIdentifier },
                    { "ClusterSize", messageChunk.Value[i].ClusterSize }
                };
                var basicProperties = DocumentChannel?.CreateBasicProperties();
                basicProperties.Headers = headers;
                DocumentChannel?.BasicPublish(DocumentCaptureExchange, "document.captured", basicProperties, new ReadOnlyMemory<byte>(bytes, 0, actualDataLength));
            }
        }
    }

    private static IModel CreateChannel(IModel? channel)
    {
        if (Connection == null)
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("amqp://captureservice:captureservice123@localhost:5672");

            Connection = connectionFactory.CreateConnection();

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                Connection.Close();
                DocumentChannel?.Close();
                ControlChannel?.Close();
            };
        }

        if (channel == null)
        {
            channel = Connection.CreateModel();
        }

        return channel;
    }

    private static void CreateExchange(string exchange, IModel? channel)
    {
        channel.ExchangeDeclare(exchange, ExchangeType.Direct, true, false);
    }
}
