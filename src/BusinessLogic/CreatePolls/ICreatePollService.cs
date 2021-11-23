using System;
using System.Threading.Tasks;

using Telegram.Bot.Types;

namespace BusinessLogic.CreatePolls
{
    public interface ICreatePollService
    {
        Task<Guid> CreateAsync(long chatId, int userId);
        Task<CreatePollRequest> FindPendingAsync(long chatId, int userId);
        Task AddPollAsync(Guid requestId, Poll poll);
    }
}