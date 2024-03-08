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

        public async Task<ScheduledPoll[]> FindNotDisabledAsync()
        {
            var dbos = await scheduledPollRepository.FindNotDisabledAsync();
            return mapper.Map<ScheduledPoll[]>(dbos);
        }
    }
}