using EnterpriseChatLibrary;

namespace EnterpriseChatServer;
internal class ServerUI
{
    private IOutputWriter _outputWriter;
    public ServerUI(IOutputWriter outputWriter)
    {
        _outputWriter = outputWriter;
    }
    public void Run()
    {
        var pipeServer = new PipeServer(_outputWriter);
        pipeServer.StartServer();
    }
}
