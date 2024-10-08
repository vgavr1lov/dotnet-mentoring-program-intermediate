using System.IO.Pipes;
using System.Security.Principal;

namespace EnterpriseChatLibrary
{
    public class ChatBot : PipeClient
    {
        private const int MaxDelay = 3000;
        private static Random random = new();
        public ChatBot(IOutputWriter outputWriter, IInputReader inputReader)
            : base(outputWriter, inputReader) { }

        public override void Start()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            ListenForCancellation(cancellationTokenSource);

            while (!IsCancellationRequested(cancellationTokenSource))
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
                            writer.WriteLine(GenrateRandomName());

                            GetMessageHistory(cancellationTokenSource, reader);

                            var messages = GetRandomMessages();

                            SendMessages(messages, cancellationTokenSource, writer, reader);
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

        private void SendMessages(List<string> messages, CancellationTokenSource cancellationTokenSource, StreamWriter writer, StreamReader reader)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                if (IsCancellationRequested(cancellationTokenSource)) break;
                writer.WriteLine(messages[i]);
                if (IsCancellationRequested(cancellationTokenSource)) break;
                Thread.Sleep(GetRandomDelay());
                if (IsCancellationRequested(cancellationTokenSource)) break;
                GetResponse(cancellationTokenSource, reader);
            }
        }

        private static string GenrateRandomName()
        {
            var randomId = random.Next(0, short.MaxValue);

            return $"user{randomId}";
        }

        private static List<string> GetRandomMessages()
        {
            var messages = new List<string>()
            {
                "Message 1",
                "Message 2",
                "Message 3",
                "Message 4",
                "Message 5",
                "Message 6",
                "Message 7",
                "Message 8",
                "Message 9",
                "Message 10"
            };

            var randomNumberOfMessage = random.Next(1, messages.Count);

            return messages.Take(randomNumberOfMessage).ToList();
        }

        private static int GetRandomDelay()
        {
            var randomDelay = random.Next(1, MaxDelay);

            return randomDelay;
        }


    }
}
