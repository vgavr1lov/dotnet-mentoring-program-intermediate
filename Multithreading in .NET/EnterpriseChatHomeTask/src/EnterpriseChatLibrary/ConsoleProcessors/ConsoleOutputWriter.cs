namespace EnterpriseChatLibrary
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
