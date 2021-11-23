using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot.Types;

namespace BusinessLogic.CreatePolls
{
    public class CreatePollService : ICreatePollService
    {
        private readonly Dictionary<Guid, CreatePollRequest> createPollRequests;

        public CreatePollService()
        {
            createPollRequests = new Dictionary<Guid, CreatePollRequest>();
        }

        public Task<Guid> CreateAsync(long chatId, int userId)
        {
            var createPollRequest = new CreatePollRequest
                {
                    Id = Guid.NewGuid(), ChatId = chatId, UserId = userId, CreateAt = DateTime.Now, IsPending = true,
                };

            createPollRequests.Add(createPollRequest.Id, createPollRequest);
            return Task.FromResult(createPollRequest.Id);
        }

        public Task<CreatePollRequest> FindPendingAsync(long chatId, int userId)
        {
            return Task.FromResult(
                createPollRequests.Values.FirstOrDefault(v => v.IsPending && v.ChatId == chatId && v.UserId == userId)
            );
        }

        public Task AddPollAsync(Guid requestId, Poll poll)
        {
            createPollRequests[requestId].Poll = poll;
            return Task.CompletedTask;
        }
    }
}