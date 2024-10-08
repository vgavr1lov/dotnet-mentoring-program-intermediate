namespace EnterpriseChatLibrary
{
    public static class MessageHistoryProcessor
    {
        private const string EndOfHistorySignal = "END_OF_HISTORY";
        public static List<MessageHistory> RecordMessage(List<MessageHistory> messageHistory, string? client, DateTime dateTime, string? message)
        {
            messageHistory.Add(new MessageHistory
            {
                Client = client,
                ReceivedAt = dateTime,
                Message = message
            });

            return messageHistory;
        }

        public static List<string> PrepareMessageHistoryForPush(List<MessageHistory> messageHistory, int numberOfMessages)
        {
            var messagesForPush = new List<string>();

            if (messageHistory.Count == 0)
            {
                messagesForPush.Add(EndOfHistorySignal);
                return messagesForPush;
            }

            var messagesForPushing = messageHistory
                .OrderBy(x => x.ReceivedAt)
                .TakeLast(numberOfMessages)
                .ToList();

            foreach (var message in messagesForPushing)
            {
                messagesForPush.Add($"{message.Client} {message.ReceivedAt}: {message.Message}");
            }

            messagesForPush.Add(EndOfHistorySignal);

            return messagesForPush;
        }

        public static bool IsEndOfHistory(string? input)
        {
            return input == EndOfHistorySignal;
        }
    }
}
