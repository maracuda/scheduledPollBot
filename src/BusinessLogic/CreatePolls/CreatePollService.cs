using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.CreatePolls
{
    public class CreatePollService : ICreatePollService
    {
        private readonly Dictionary<Guid, CreatePollRequest> createPollRequests;

        public CreatePollService()
        {
            createPollRequests = new Dictionary<Guid, CreatePollRequest>();
        }

        public Task CreateAsync(CreatePollRequest createPollRequest)
        {
            createPollRequests.Add(createPollRequest.Id, createPollRequest);
            return Task.FromResult(createPollRequest.Id);
        }

        public Task<Guid> CreateAsync(long chatId, int userId)
        {
            var createPollRequest = new CreatePollRequest
                {
                    Id = Guid.NewGuid(),
                    ChatId = chatId,
                    UserId = userId,
                    CreateAt = DateTime.Now,
                    IsPending = true,
                };

            createPollRequests.Add(createPollRequest.Id, createPollRequest);
            return Task.FromResult(createPollRequest.Id);
        }

        public Task<CreatePollRequest> FindPendingAsync(long chatId, int userId)
        {
            return Task.FromResult(
                createPollRequests
                    .Values
                    .Where(v => v.IsPending && v.ChatId == chatId && v.UserId == userId)
                    .OrderByDescending(v => v.CreateAt)
                    .FirstOrDefault()
            );
        }

        public Task SaveAsync(CreatePollRequest pendingRequest)
        {
            createPollRequests[pendingRequest.Id] = pendingRequest;
            return Task.CompletedTask;
        }

        public Task<CreatePollRequest> FindPendingAndValidAsync(int fromId)
        {
            return Task.FromResult(createPollRequests.Values.FirstOrDefault(v =>
                                                                                v.UserId == fromId
                                                                                && v.IsPending
                                                                                && v.IsValid
                                   )
            );
        }
    }
}