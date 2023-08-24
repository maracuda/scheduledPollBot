using System;
using System.Linq;
using System.Threading.Tasks;

using BusinessLogic;
using BusinessLogic.CreatePolls;

using Microsoft.EntityFrameworkCore;

namespace TelegramInteraction.Chat;

public class PaymentsRepository : IPaymentsRepository
{
    public PaymentsRepository(IPollContextFactory pollContextFactory)
    {
        this.pollContextFactory = pollContextFactory;
    }

    public async Task<PaymentDbo[]> SearchActiveAsync(long[] chatIds)
    {
        var paymentDbos = pollContextFactory.Create().Payments;

        var payments = await paymentDbos
                             .Where(p => chatIds.Contains(p.ChatId))
                             .ToArrayAsync();
        return payments.Where(p => p.Date < DateTime.UtcNow && DateTime.UtcNow < CalculateEndDate(p)).ToArray();
    }

    public async Task CreateAsync(PaymentDbo paymentDbo)
    {
        var pollsContext = pollContextFactory.Create();
        var paymentDbos = pollsContext.Payments;

        await paymentDbos.AddAsync(paymentDbo);

        await pollsContext.SaveChangesAsync();
    }

    private static DateTime CalculateEndDate(PaymentDbo p)
    {
        switch(p.Period)
        {
        case PeriodType.Month:
            return p.Date.AddMonths(p.PeriodsCount);
        case PeriodType.Year:
            return p.Date.AddYears(p.PeriodsCount);
        default:
            throw new ArgumentOutOfRangeException();
        }
    }

    private readonly IPollContextFactory pollContextFactory;
}