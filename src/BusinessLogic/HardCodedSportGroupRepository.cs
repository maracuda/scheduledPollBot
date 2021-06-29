using System.Threading.Tasks;

namespace BusinessLogic
{
    public class HardCodedSportGroupRepository : ISportGroupRepository
    {
        public Task<SportGroup[]> ReadAllAsync()
        {
            return Task.FromResult(new[]
                    {
                        new SportGroup
                            {
                                Name = "A.Petrazhickiy",
                                TrainingSchedule = "0 5 * * 2,5",
                                NotificationSchedule = "0 5 * * 1,4",
                                TelegramChatId = "-1001318882712",
                                Title = "Йога",
                            },
                        new SportGroup
                            {
                                Name = "E.Petrazhickaya",
                                TrainingSchedule = "0 5 * * 2,4",
                                NotificationSchedule = "0 5 * * 1,3",
                                TelegramChatId = "-1001286172835",
                                Title = "Йога",
                            },
                    }
            );
        }
    }
}