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
                                NotificationSchedule = "0 5 * * 1,4",
                                TelegramChatId = "-1001318882712",
                            },
                        new SportGroup
                            {
                                Name = "E.Petrazhickaya",
                                NotificationSchedule = "0 5 * * 1,3",
                                TelegramChatId = "-1001286172835",
                            },
                    }
            );
        }
    }
}