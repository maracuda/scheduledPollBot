using System.Threading.Tasks;

namespace BusinessLogic.CreatePolls
{
    public interface ICreatePollService
    {
        Task CreateAsync(CreatePollRequest createPollRequest);
        Task<CreatePollRequest> FindPendingAsync(long chatId, int userId);
        Task SaveAsync(CreatePollRequest pendingRequest);
        Task<CreatePollRequest> FindPendingAndValidAsync(int fromId);
    }
}