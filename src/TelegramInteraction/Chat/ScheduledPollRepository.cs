using System.Linq;
using System.Threading.Tasks;

using BusinessLogic;
using BusinessLogic.CreatePolls;

using Microsoft.EntityFrameworkCore;

namespace TelegramInteraction.Chat
{
    public class ScheduledPollRepository : IScheduledPollRepository
    {
        private readonly IPollContextFactory pollContextFactory;

        public ScheduledPollRepository(IPollContextFactory pollContextFactory)
        {
            this.pollContextFactory = pollContextFactory;
        }

        public async Task CreateAsync(ScheduledPollDbo poll)
        {
            await using var context = pollContextFactory.Create();

            await context.Polls.AddAsync(poll);
            await context.SaveChangesAsync();
        }

        public async Task<ScheduledPollDbo[]> FindAsync(long chatId)
        {
            await using var context = pollContextFactory.Create();

            return await context.Polls.Where(p => p.ChatId == chatId).ToArrayAsync();
        }

        public async Task SaveAsync(ScheduledPollDbo poll)
        {
            await using var context = pollContextFactory.Create();

            context.Polls.Update(poll);
            await context.SaveChangesAsync();
        }

        public async Task<ScheduledPollDbo[]> FindNotDisabledAsync()
        {
            await using var context = pollContextFactory.Create();

            return await context.Polls.Where(p => !p.IsDisabled).ToArrayAsync();
        }
    }
}