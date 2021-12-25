using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.CreatePolls
{
    public class CreatePollService : ICreatePollService
    {
        public CreatePollService(ICreatePollRepository createPollRepository)
        {
            this.createPollRepository = createPollRepository;
        }

        public async Task CreateAsync(CreatePollRequest createPollRequest)
        {
            await createPollRepository.CreateAsync(createPollRequest);
        }

        public async Task<CreatePollRequest> FindPendingAsync(long chatId, int userId)
        {
            var userPolls = await createPollRepository.FindAsync(userId);

            return userPolls
                   .Where(v => v.IsPending && v.ChatId == chatId)
                   .OrderByDescending(v => v.CreateAt)
                   .FirstOrDefault();
        }

        public async Task SaveAsync(CreatePollRequest pendingRequest)
        {
            await createPollRepository.SaveAsync(pendingRequest);
        }

        public async Task<CreatePollRequest> FindPendingAndValidAsync(int fromId)
        {
            var userPolls = await createPollRepository.FindAsync(fromId);

            return userPolls.FirstOrDefault(v => v.IsPending && v.IsValid);
        }

        private readonly ICreatePollRepository createPollRepository;
    }
}