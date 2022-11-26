using System.Threading.Tasks;

namespace BusinessLogic.CreatePolls;

public interface IS3ClientWrapper
{
    Task<TItem[]> ReadAllAsync<TItem>(string bucketName, string key);
    Task WriteAsync<TItem>(TItem[] values, string bucketName, string key);
}