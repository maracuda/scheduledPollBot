using System.Threading.Tasks;

using AutoMapper;

using BusinessLogic;

namespace TelegramInteraction.Chat
{
    public class ScheduledPollService : IScheduledPollService
    {
        private readonly IScheduledPollRepository scheduledPollRepository;
        private readonly Mapper mapper;

        public ScheduledPollService(
            IScheduledPollRepository scheduledPollRepository
        )
        {
            this.scheduledPollRepository = scheduledPollRepository;
            mapper = new Mapper(new MapperConfiguration(
                                    c => c.CreateMap<ScheduledPollDbo, ScheduledPoll>()
                                          .ReverseMap()
                                )
            );
        }

        public async Task CreateAsync(ScheduledPoll scheduledPoll)
        {
            await scheduledPollRepository.CreateAsync(mapper.Map<ScheduledPollDbo>(scheduledPoll));
        }

        public async Task<ScheduledPoll[]> FindNotDisabledAsync()
        {
            var dbos = await scheduledPollRepository.FindNotDisabledAsync();
            return mapper.Map<ScheduledPoll[]>(dbos);
        }

        public async Task<ScheduledPoll[]> GetAll(long chatId)
        {
            var dbos = await scheduledPollRepository.FindAsync(chatId);
            return mapper.Map<ScheduledPoll[]>(dbos);
        }

        public async Task SaveAsync(ScheduledPoll poll)
        {
            await scheduledPollRepository.SaveAsync(mapper.Map<ScheduledPollDbo>(poll));
        }
    }
}