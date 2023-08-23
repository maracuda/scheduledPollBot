using System;
using System.Threading.Tasks;

namespace BusinessLogic.CreatePolls
{
    public interface ICreatePollRepository
    {
        Task CreateAsync(CreatePollRequest createPollRequest);
        Task<CreatePollRequest[]> FindAsync(long userId);
        Task SaveAsync(CreatePollRequest pendingRequest);
        Task<CreatePollRequest[]> ReadManyAsync(Guid[] ids);
    }
}