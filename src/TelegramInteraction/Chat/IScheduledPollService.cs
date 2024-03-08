using System.Threading.Tasks;

namespace TelegramInteraction.Chat
{
    public interface IScheduledPollService
    {
        Task<ScheduledPoll[]> FindNotDisabledAsync();
    }
}