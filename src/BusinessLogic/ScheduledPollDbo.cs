using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic
{
    [Table("polls")]
    public class ScheduledPollDbo
    {
        public Guid Id { get; set; }
        public Guid CreatedRequestId { get; set; }
        public string[] Options { get; set; }
        public bool IsAnonymous { get; set; }
        public string Name { get; set; }
        public string Schedule { get; set; }
        public long ChatId { get; set; }
        public bool IsDisabled { get; set; }
    }
}