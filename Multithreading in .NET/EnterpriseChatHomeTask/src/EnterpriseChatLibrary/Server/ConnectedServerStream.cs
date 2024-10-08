using System.IO.Pipes;

namespace EnterpriseChatLibrary
{
    internal class ConnectedServerStream
    {
        public NamedPipeServerStream ServerStream { get; set; }
        public StreamWriter ServerStreamWriter { get; set; }
        public StreamReader ServerStreamReader { get; set; }
    }
}
