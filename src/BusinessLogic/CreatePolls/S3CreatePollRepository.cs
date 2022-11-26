using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.CreatePolls;

public class S3CreatePollRepository : ICreatePollRepository
{
    public S3CreatePollRepository(
        IS3ClientWrapper s3ClientWrapper
    )
    {
        this.s3ClientWrapper = s3ClientWrapper;
    }

    public async Task CreateAsync(CreatePollRequest createPollRequest)
    {
        var allPolls = await s3ClientWrapper.ReadAllAsync<CreatePollRequest>(key);
        await s3ClientWrapper.WriteAsync(allPolls.Append(createPollRequest).ToArray(), key);
    }

    public async Task<CreatePollRequest[]> FindAsync(long userId)
    {
        var allPolls = await s3ClientWrapper.ReadAllAsync<CreatePollRequest>(key);
        return allPolls.Where(p => p.UserId == userId).ToArray();
    }

    public async Task SaveAsync(CreatePollRequest pendingRequest)
    {
        var allPolls =
            (await s3ClientWrapper.ReadAllAsync<CreatePollRequest>(key)).ToDictionary(p => p.Id);

        allPolls[pendingRequest.Id] = pendingRequest;

        var allPollsValues = allPolls.Values.ToArray();
        await s3ClientWrapper.WriteAsync(allPollsValues, key);
    }

    private const string key = "requests";

    private readonly IS3ClientWrapper s3ClientWrapper;
}