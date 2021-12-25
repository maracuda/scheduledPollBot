using System.Threading.Tasks;

using BusinessLogic;

namespace TelegramInteraction.Chat
{
    public interface IScheduledPollRepository
    {
        Task CreateAsync(ScheduledPollDbo poll);
        Task<ScheduledPollDbo[]> FindAsync(long chatId);
        Task SaveAsync(ScheduledPollDbo poll);
        Task<ScheduledPollDbo[]> FindNotDisabledAsync();
    }
}