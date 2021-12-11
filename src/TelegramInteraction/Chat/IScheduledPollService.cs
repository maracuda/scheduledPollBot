using System.Threading.Tasks;

namespace TelegramInteraction.Chat
{
    public interface IScheduledPollService
    {
        Task CreateAsync(ScheduledPoll scheduledPoll);
        Task<ScheduledPoll[]> ReadAllAsync();
    }
}