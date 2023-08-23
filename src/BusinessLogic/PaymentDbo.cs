using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic;

[Table("payments")]
public class PaymentDbo
{
    public Guid Id { get; set; }
    public long ChatId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public PeriodType Period { get; set; }
    public int PeriodsCount { get; set; }
}