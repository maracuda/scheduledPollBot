using System.Threading.Tasks;

using BusinessLogic;

namespace TelegramInteraction.Chat;

public interface IPaymentsRepository
{
    Task<PaymentDbo[]> SearchActiveAsync(long[] chatIds);
    Task CreateAsync(PaymentDbo paymentDbo);
}