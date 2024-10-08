namespace EnterpriseChatLibrary
{
    public class ConsoleInputReader : IInputReader
    {
        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }
    }
}
