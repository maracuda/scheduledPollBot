using System.Threading.Tasks;

namespace BusinessLogic.CreatePolls
{
    public interface ICreatePollService
    {
        Task CreateAsync(CreatePollRequest createPollRequest);
        Task<CreatePollRequest> FindPendingAsync(long chatId, long userId);
        Task SaveAsync(CreatePollRequest pendingRequest);
        Task<CreatePollRequest> FindPendingAndValidAsync(long fromId);
    }
}