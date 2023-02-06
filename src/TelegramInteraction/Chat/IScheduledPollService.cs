using System;
using System.Threading.Tasks;

namespace TelegramInteraction.Chat
{
    public interface IScheduledPollService
    {
        Task CreateAsync(ScheduledPoll scheduledPoll);
        Task<ScheduledPoll[]> FindNotDisabledAsync();
        Task<ScheduledPoll[]> GetAll(long chatId);
        Task SaveAsync(ScheduledPoll poll);
        Task<ScheduledPoll> ReadAsync(Guid pollId);
    }
}