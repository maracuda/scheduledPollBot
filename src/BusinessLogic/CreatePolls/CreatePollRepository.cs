using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.CreatePolls
{
    public class CreatePollRepository : ICreatePollRepository
    {
        public CreatePollRepository(IPollContextFactory pollContextFactory)
        {
            this.pollContextFactory = pollContextFactory;
            mapper = new Mapper(new MapperConfiguration(
                                    c => c.CreateMap<CreatePollRequestDbo, CreatePollRequest>()
                                          .ReverseMap()
                                )
            );
        }

        public async Task CreateAsync(CreatePollRequest createPollRequest)
        {
            await using var pollsContext = pollContextFactory.Create();
            var dbo = mapper.Map<CreatePollRequestDbo>(createPollRequest
            );

            await pollsContext.Requests.AddAsync(dbo);
            await pollsContext.SaveChangesAsync();
        }

        public async Task<CreatePollRequest[]> FindAsync(int userId)
        {
            await using var pollsContext = pollContextFactory.Create();
            var dbos = await pollsContext.Requests.Where(p => p.UserId == userId).ToArrayAsync();
            return mapper.Map<CreatePollRequest[]>(dbos);
        }

        public async Task SaveAsync(CreatePollRequest pendingRequest)
        {
            var dbo = mapper.Map<CreatePollRequestDbo>(pendingRequest);

            await using var pollsContext = pollContextFactory.Create();
            pollsContext.Requests.Update(dbo);
            await pollsContext.SaveChangesAsync();
        }

        private readonly Mapper mapper;
        private readonly IPollContextFactory pollContextFactory;
    }
}