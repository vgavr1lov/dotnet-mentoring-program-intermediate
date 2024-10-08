using System.IO.Pipes;

namespace EnterpriseChatLibrary;

public class PipeServer
{
    protected const string NamedPipeServer = "MyPipe";
    private const int NumberOfMessagesToStore = 10;
    private static List<MessageHistory> messageHistory = new();
    private static List<ConnectedServerStream> connectedServerStreams = new();
    private static object connectionLock = new();
    private IOutputWriter _outputWriter;

    public PipeServer(IOutputWriter outputWriter) 
    {
        _outputWriter = outputWriter;
    }

    public void StartServer()
    {
        while (true)
        {
            WaitForConnection();
        }
    }


    private void WaitForConnection()
    {
        var server = new NamedPipeServerStream(NamedPipeServer, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous);

        _outputWriter.Print("Waiting for connection...");
        server.WaitForConnection();
        RecordEstablishedConnection(server);

        var clientThread = new Thread(() =>
        {
            CommunicateWithClient(server);
        });
        clientThread.Start();
    }

    private void RecordEstablishedConnection(NamedPipeServerStream server)
    {
        lock (connectionLock)
        {
            connectedServerStreams.Add(new ConnectedServerStream
            {
                ServerStream = server,
                ServerStreamWriter = new StreamWriter(server),
                ServerStreamReader = new StreamReader(server),
            });
        }
    }

    private void RemoveEstablishedConnection(NamedPipeServerStream server)
    {
        lock (connectionLock)
        {
            var connectedServerStream = connectedServerStreams
                .Single(s => s.ServerStream == server);

            connectedServerStreams.Remove(connectedServerStream);
        }
    }
    private StreamWriter GetStreamWriter(NamedPipeServerStream server)
    {
        lock (connectionLock)
        {
            var streamWriter = connectedServerStreams
            .Where(s => s.ServerStream == server)
            .Select(s => s.ServerStreamWriter)
            .First();

            streamWriter.AutoFlush = true;

            return streamWriter;
        }
    }

    private StreamReader GetStreamReader(NamedPipeServerStream server)
    {
        lock (connectionLock)
        {
            var streamReader = connectedServerStreams
            .Where(s => s.ServerStream == server)
            .Select(s => s.ServerStreamReader)
            .First();

            return streamReader;
        }
    }

    private void CommunicateWithClient(NamedPipeServerStream server)
    {
        if (server == null)
            return;

        var clientName = string.Empty;

        try
        {
            var reader = GetStreamReader(server);
            var writer = GetStreamWriter(server);

            clientName = reader.ReadLine();
            _outputWriter.Print($"New connection with {clientName} {DateTime.Now}");

            BroadcastMessageHistory(writer);

            while (server.IsConnected)
            {
                string? lineFromClient = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(lineFromClient))
                    break;

                var receivedAt = DateTime.Now;
                var message = $"{clientName} {receivedAt}: {lineFromClient}";

                lock (messageHistory)
                {
                    MessageHistoryProcessor.RecordMessage(messageHistory, clientName, receivedAt, lineFromClient);
                }

                var task = Task.Run(() =>
                {
                    BroadcastToClients(message, server);
                });

                _outputWriter.Print(message);
                writer.WriteLine($"Message: {lineFromClient} recieved {DateTime.Now}");

            }
        }
        catch (IOException ex)
        {
            _outputWriter.Print($"Client {clientName} is disconnected: {ex.Message}");
        }
        finally
        {
            lock (connectionLock)
            {
                RemoveEstablishedConnection(server);
            }

            if (server.IsConnected)
            {
                server.Flush();           
            }
            server.Close();
            server.Dispose();
        }
    }

    private void BroadcastMessageHistory(StreamWriter writer)
    {
        List<string> preparedMessageHistory;
        lock (messageHistory)
        {
            preparedMessageHistory = MessageHistoryProcessor.PrepareMessageHistoryForPush(messageHistory, NumberOfMessagesToStore);
        }

        foreach (var message in preparedMessageHistory)
        {
            writer.WriteLine(message);
        }
    }

    private void BroadcastToClients(string message, NamedPipeServerStream senderClient)
    {
        lock (connectionLock)
        {
            foreach (var connectedServerStream in connectedServerStreams)
            {
                if (connectedServerStream.ServerStream != senderClient && connectedServerStream.ServerStream.IsConnected)
                {
                    try
                    {
                        connectedServerStream.ServerStreamWriter.AutoFlush = true;
                        connectedServerStream.ServerStreamWriter.WriteLine(message);
                    }
                    catch (IOException ex)
                    {
                        _outputWriter.Print($"{ex.Message}");
                    }
                }
            }
        }
    }
}
