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
                            TrainingSchedule = applicationSettings.GetString("TrainingSchedule"),
                            NotificationSchedule = applicationSettings.GetString("NotificationSchedule"),
                            TelegramChatId = applicationSettings.GetString("ChatId"),
                            Title = "Title",
                        },
                });
        }

        private readonly IApplicationSettings applicationSettings;
    }
}