using System.Linq;
using System.Threading.Tasks;

using BusinessLogic;
using BusinessLogic.CreatePolls;

namespace TelegramInteraction.Chat;

public class S3ScheduledPollRepository : IScheduledPollRepository
{
    public S3ScheduledPollRepository(
        IS3ClientWrapper s3ClientWrapper
    )
    {
        this.s3ClientWrapper = s3ClientWrapper;
    }

    public async Task CreateAsync(ScheduledPollDbo poll)
    {
        var allPolls = await s3ClientWrapper.ReadAllAsync<ScheduledPollDbo>(key);
        await s3ClientWrapper.WriteAsync(allPolls.Append(poll).ToArray(), key);
    }

    public async Task<ScheduledPollDbo[]> FindAsync(long chatId)
    {
        var allPolls = await s3ClientWrapper.ReadAllAsync<ScheduledPollDbo>(key);
        return allPolls.Where(p => p.ChatId == chatId).ToArray();
    }

    public async Task SaveAsync(ScheduledPollDbo poll)
    {
        var allPolls =
            (await s3ClientWrapper.ReadAllAsync<ScheduledPollDbo>(key)).ToDictionary(p => p.Id);

        allPolls[poll.Id] = poll;

        var allPollsValues = allPolls.Values.ToArray();
        await s3ClientWrapper.WriteAsync(allPollsValues, key);
    }

    public async Task<ScheduledPollDbo[]> FindNotDisabledAsync()
    {
        var allPolls = await s3ClientWrapper.ReadAllAsync<ScheduledPollDbo>(key);
        return allPolls.Where(p => !p.IsDisabled).ToArray();
    }

    private const string key = "polls";

    private readonly IS3ClientWrapper s3ClientWrapper;
}