using EnterpriseChatLibrary;

namespace EnterpriseChatClient
{
    internal class ClientUI
    {
        private IOutputWriter _outputWriter;
        private IInputReader _inputReader;
        public ClientUI(IOutputWriter outputWriter, IInputReader inputReader)
        {
            _outputWriter = outputWriter;
            _inputReader = inputReader;
        }
        public void Run()
        {
            var pipeClient = new PipeClient(_outputWriter, _inputReader);
            pipeClient.Start();
        }
    }
}
