using System;

using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class ScheduledPoll
    {
        public Guid Id { get; set; }
        public Guid CreatedRequestId { get; set; }
        public string[] Options { get; set; }
        public bool IsAnonymous { get; set; }
        public string Name { get; set; }
        public string Schedule { get; set; }
        public ChatId ChatId { get; set; }
    }
}