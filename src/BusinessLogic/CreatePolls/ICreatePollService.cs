using System;
using System.Threading.Tasks;

namespace BusinessLogic.CreatePolls
{
    public interface ICreatePollService
    {
        Task CreateAsync(CreatePollRequest createPollRequest);
        Task<Guid> CreateAsync(long chatId, int userId);
        Task<CreatePollRequest> FindPendingAsync(long chatId, int userId);
        Task SaveAsync(CreatePollRequest pendingRequest);
        Task<CreatePollRequest> FindPendingAsync(int fromId);
    }
}