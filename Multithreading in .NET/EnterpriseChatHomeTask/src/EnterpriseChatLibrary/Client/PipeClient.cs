using System.IO.Pipes;
using System.Security.Principal;

namespace EnterpriseChatLibrary;

public class PipeClient
{
    protected const string NamedPipeServer = "MyPipe";
    protected IOutputWriter _outputWriter;
    protected IInputReader _inputReader;

    public PipeClient(IOutputWriter outputWriter, IInputReader inputReader)
    {
        _outputWriter = outputWriter;
        _inputReader = inputReader;
    }

    public virtual void Start()
    {
        var cancellationTokenSource = new CancellationTokenSource();

        ListenForCancellation(cancellationTokenSource);

        while (!cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                using (var client = new NamedPipeClientStream(".", NamedPipeServer, PipeDirection.InOut, PipeOptions.Asynchronous, TokenImpersonationLevel.Impersonation))
                {
                    if (IsCancellationRequested(cancellationTokenSource)) break;
                    client.Connect();


                    using (var reader = new StreamReader(client))
                    using (var writer = new StreamWriter(client) { AutoFlush = true })
                    {
                        if (IsCancellationRequested(cancellationTokenSource)) break;
                        writer.WriteLine("Valentin");

                        GetMessageHistory(cancellationTokenSource, reader);

                        while (client.IsConnected && !IsCancellationRequested(cancellationTokenSource))
                        {
                            GetResponse(cancellationTokenSource, reader);
                        }

                    }
                }
            }
            catch (IOException ex)
            {
                _outputWriter.Print(ex.Message);
                break;
            }
        }
    }

    protected void GetResponse(CancellationTokenSource cancellationTokenSource, StreamReader reader)
    {
        string? lineFromServer = reader.ReadLine();
        if (!string.IsNullOrWhiteSpace(lineFromServer))
        {
            if (IsCancellationRequested(cancellationTokenSource)) return;
            _outputWriter.Print($"{NamedPipeServer}: {lineFromServer}");
        }
    }

    protected void ListenForCancellation(CancellationTokenSource cancellationTokenSource)
    {
        Task.Run(() =>
        {
            while (!IsCancellationRequested(cancellationTokenSource))
            {
                if (_inputReader.ReadKey().Key == ConsoleKey.X)
                {
                    cancellationTokenSource.Cancel();
                    _outputWriter.Print($"Disconnected");
                };
            }
        }, cancellationTokenSource.Token);
    }
    protected void GetMessageHistory(CancellationTokenSource cancellationTokenSource, StreamReader reader)
    {
        while (true)
        {
            var historyMessage = reader.ReadLine();
            if (MessageHistoryProcessor.IsEndOfHistory(historyMessage)) break;
            if (IsCancellationRequested(cancellationTokenSource)) break;
            _outputWriter.Print($"{NamedPipeServer}: {historyMessage}");
        }
    }

    protected bool IsCancellationRequested(CancellationTokenSource cancellationTokenSource)
    {
        return cancellationTokenSource.Token.IsCancellationRequested;
    }
}
