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

public class S3ClientWrapper : IS3ClientWrapper
{
    public S3ClientWrapper(
        IAmazonS3 yandexAws,
        IApplicationSettings applicationSettings
    )
    {
        bucketName = applicationSettings.GetString("BucketName");
        this.yandexAws = yandexAws;
    }

    public async Task<TItem[]> ReadAllAsync<TItem>(string key)
    {
        var allObjects = await yandexAws.ListObjectsAsync(bucketName);
        if(allObjects.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"Can't list object in bucket {bucketName}");
        }

        if(allObjects.S3Objects.FirstOrDefault(o => o.Key == key) == null)
        {
            return Array.Empty<TItem>();
        }

        var response = await yandexAws.GetObjectAsync(bucketName, key);
        if(response.HttpStatusCode == HttpStatusCode.OK)
        {
            using(var reader = new StreamReader(response.ResponseStream))
            {
                var readToEndAsync = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<TItem[]>(readToEndAsync);
            }
        }

        throw new Exception($"Can't read object {key} in bucket {bucketName}");
    }

    public Task WriteAsync<TItem>(TItem[] values, string key)
    {
        lock(locker)
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(values)));
            yandexAws.UploadObjectFromStreamAsync(bucketName,
                                                  key,
                                                  memoryStream,
                                                  new Dictionary<string, object>()
                     )
                     .GetAwaiter()
                     .GetResult();
        }

        return Task.CompletedTask;
    }

    private volatile object locker = new();
    private readonly IAmazonS3 yandexAws;
    private readonly string bucketName;
}