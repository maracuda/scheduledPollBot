using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface ISportGroupRepository
    {
        Task<SportGroup[]> ReadAllAsync();
    }
}