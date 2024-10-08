using EnterpriseChatLibrary;

namespace EnterpriseChatClientBot;
internal class ChatBotUI
{
    private IOutputWriter _outputWriter;
    private IInputReader _inputReader;
    public ChatBotUI(IOutputWriter outputWriter, IInputReader inputReader)
    {
        _outputWriter = outputWriter;
        _inputReader = inputReader;
    }
    public void Run()
    {
        var pipeChatBot = new ChatBot(_outputWriter, _inputReader);
        pipeChatBot.Start();
    }
}
