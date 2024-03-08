using System.Threading.Tasks;

using BusinessLogic;

namespace TelegramInteraction.Chat
{
    public interface IScheduledPollRepository
    {
        Task<ScheduledPollDbo[]> FindNotDisabledAsync();
    }
}