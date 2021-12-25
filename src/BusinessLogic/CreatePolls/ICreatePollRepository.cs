using System.Threading.Tasks;

namespace BusinessLogic.CreatePolls
{
    public interface ICreatePollRepository
    {
        Task CreateAsync(CreatePollRequest createPollRequest);
        Task<CreatePollRequest[]> FindAsync(int userId);
        Task SaveAsync(CreatePollRequest pendingRequest);
    }
}