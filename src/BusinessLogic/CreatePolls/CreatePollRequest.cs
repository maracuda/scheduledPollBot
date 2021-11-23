using System;

using Telegram.Bot.Types;

namespace BusinessLogic.CreatePolls
{
    public class CreatePollRequest
    {
        public Guid Id { get; set; }
        public long ChatId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsPending { get; set; }
        public Poll Poll { get; set; }
    }
}