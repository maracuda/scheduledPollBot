using System;

namespace BusinessLogic.CreatePolls
{
    public class CreatePollRequest
    {
        public Guid Id { get; set; }
        public long ChatId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsPending { get; set; }
        public string PollName { get; set; }
        public bool IsAnonymous { get; set; }
        public string[] Options { get; set; }
        public string Schedule { get; set; }
    }
}