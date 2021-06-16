using System.Threading.Tasks;

namespace BusinessLogic
{
    public class DevelopSportGroupRepository : ISportGroupRepository
    {
        public DevelopSportGroupRepository(
            IApplicationSettings applicationSettings
        )
        {
            this.applicationSettings = applicationSettings;
        }

        public Task<SportGroup[]> ReadAllAsync()
        {
            return Task.FromResult(new[]
                {
                    new SportGroup
                        {
                            Name = "TestGroup",
                            NotificationSchedule = applicationSettings.GetString("Schedule"),
                            TelegramChatId = applicationSettings.GetString("ChatId"),
                        },
                });
        }

        private readonly IApplicationSettings applicationSettings;
    }
}