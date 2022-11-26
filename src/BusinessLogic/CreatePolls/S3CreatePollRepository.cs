using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Amazon.S3;

using Newtonsoft.Json;

namespace BusinessLogic.CreatePolls;

public class S3CreatePollRepository : ICreatePollRepository
{
    public S3CreatePollRepository(
        IAmazonS3 yandexAws
    )
    {
        this.yandexAws = yandexAws;
    }

    public async Task CreateAsync(CreatePollRequest createPollRequest)
    {
        var allPolls = await ReadAllAsync();
        await UploadAsync(allPolls.Append(createPollRequest).ToArray());
    }

    public async Task<CreatePollRequest[]> FindAsync(long userId)
    {
        var allPolls = await ReadAllAsync();
        return allPolls.Where(p => p.UserId == userId).ToArray();
    }

    public async Task SaveAsync(CreatePollRequest pendingRequest)
    {
        var allPolls = (await ReadAllAsync()).ToDictionary(p => p.Id);

        allPolls[pendingRequest.Id] = pendingRequest;

        var allPollsValues = allPolls.Values.ToArray();
        await UploadAsync(allPollsValues);
    }

    private async Task UploadAsync(CreatePollRequest[] allPollsValues)
    {
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(allPollsValues)));
        await yandexAws.UploadObjectFromStreamAsync(bucketName,
                                                    requests,
                                                    memoryStream,
                                                    new Dictionary<string, object>()
        );
    }

    private async Task<CreatePollRequest[]> ReadAllAsync()
    {
        var response = await yandexAws.GetObjectAsync(bucketName, requests);
        if(response.HttpStatusCode == HttpStatusCode.OK)
        {
            using(var reader = new StreamReader(response.ResponseStream))
            {
                var readToEndAsync = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<CreatePollRequest[]>(readToEndAsync);
            }
        }

        return Array.Empty<CreatePollRequest>();
    }

    private readonly IAmazonS3 yandexAws;
    private static readonly string bucketName = "testpolls";
    private static readonly string requests = "requests";
}