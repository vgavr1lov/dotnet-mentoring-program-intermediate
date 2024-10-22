using DataLibrary;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ProcessingService;

internal class Program
{
    private const string IncomingDocumentDirectory = @"IncomingDocuments";
    private const string Exchange = "DocumentCaptureExchange";
    private const string Queue = "processingServiceQueue";

    static void Main(string[] args)
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqp://processingservice:processingservice123@localhost:5672");

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        channel.QueueDeclare(Queue, true, false, false);

        channel.QueueBind(Queue, Exchange, "document.captured");

        var messageChunks = new Dictionary<string, List<PreparedMessageBytes>>();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) =>
        {
            var content = args.Body.ToArray();
            var headers = args.BasicProperties.Headers;
            var sequenceIdentifier = Encoding.UTF8.GetString((byte[])headers["SequenceIdentifier"]);
            var positionIdentifier = Convert.ToInt32(headers["PositionIdentifier"]);
            var clusterSize = Convert.ToInt32(headers["ClusterSize"]);

            if (!messageChunks.ContainsKey(sequenceIdentifier))
                messageChunks[sequenceIdentifier] = new List<PreparedMessageBytes>();

            messageChunks[sequenceIdentifier].Add(new PreparedMessageBytes
            {
                DataBytes = content,
                PositionIdentifier = positionIdentifier,
                ClusterSize = clusterSize
            });

            if (clusterSize == messageChunks[sequenceIdentifier].Count)
            {
                var sequencedMessageChunks = messageChunks[sequenceIdentifier].OrderBy(x => x.PositionIdentifier).ToList();
                var fullMessage = ReassembleData(sequencedMessageChunks);
                SaveMessage(sequenceIdentifier, fullMessage);
            }

        };

        channel.BasicConsume(Queue, true, consumer);

        Console.ReadLine();

        channel.Close();
        connection.Close();
    }

    private static void SaveMessage(string messageId, byte[] content)
    {
        var fileName = messageId;
        var directory = Path.Combine(IncomingDocumentDirectory, fileName);
        File.WriteAllBytes(directory, content);
    }

    private static byte[] ReassembleData(List<PreparedMessageBytes> sequencedMessageChunks)
    {
        var totalNumberOfChunks = sequencedMessageChunks.Count;
        var reassembledChunks = 0;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            foreach (var messageChunk in sequencedMessageChunks)
            {
                if (messageChunk.DataBytes != null)
                    memoryStream.Write(messageChunk.DataBytes, 0, messageChunk.DataBytes.Length);
                reassembledChunks++;
                UpdateReassemblingProgress(totalNumberOfChunks, reassembledChunks);
            }
            return memoryStream.ToArray();
        }
    }

    private static void UpdateReassemblingProgress(int totalNumberOfChunks, int reassembledChunks)
    {
        Console.Clear();
        var reassembledChunksMessage = $"Reassembled {reassembledChunks}/{totalNumberOfChunks} message chunks.";
        Console.WriteLine(reassembledChunksMessage);
    }

}
