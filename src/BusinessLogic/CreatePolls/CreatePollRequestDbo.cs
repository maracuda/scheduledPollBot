using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.CreatePolls
{
    [Table("requests")]
    public class CreatePollRequestDbo
    {
        public Guid Id { get; set; }
        public long ChatId { get; set; }
        public long UserId { get; set; }
        
        [Column(TypeName = "timestamp")]
        public DateTime CreateAt { get; set; }
        public bool IsPending { get; set; }
        public string PollName { get; set; }
        public bool IsAnonymous { get; set; }
        public string[] Options { get; set; }
        public string Schedule { get; set; }
        public bool IsValid { get; set; }
        public bool AllowsMultipleAnswers { get; set; }
    }
}