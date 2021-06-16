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
                                NotificationSchedule = "0 5 * * 1,4",
                                TelegramChatId = "-1001318882712",
                            },
                    }
            );
        }
    }
}