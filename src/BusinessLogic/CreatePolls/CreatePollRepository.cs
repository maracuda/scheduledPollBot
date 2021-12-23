using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.CreatePolls
{
    public class CreatePollRepository : ICreatePollRepository
    {
        private readonly Mapper mapper;

        public CreatePollRepository()
        {
            mapper = new Mapper(new MapperConfiguration(
                                    c => c.CreateMap<CreatePollRequestDbo, CreatePollRequest>()
                                          .ReverseMap()
                                )
            );
        }

        public async Task CreateAsync(CreatePollRequest createPollRequest)
        {
            using(var pollsContext = new PollsContext())
            {
                var dbo = mapper.Map<CreatePollRequestDbo>(createPollRequest
                );

                await pollsContext.Requests.AddAsync(dbo);
                await pollsContext.SaveChangesAsync();
            }
        }

        public async Task<CreatePollRequest[]> FindAsync(int userId)
        {
            using(var pollsContext = new PollsContext())
            {
                var dbos = await pollsContext.Requests.Where(p => p.UserId == userId).ToArrayAsync();
                return mapper.Map<CreatePollRequest[]>(dbos);
            }
        }

        public async Task SaveAsync(CreatePollRequest pendingRequest)
        {
            var dbo = mapper.Map<CreatePollRequestDbo>(pendingRequest);

            using(var pollsContext = new PollsContext())
            {
                pollsContext.Requests.Update(dbo);
                await pollsContext.SaveChangesAsync();
            }
        }
    }

    public class PollsContext : DbContext
    {
        public DbSet<CreatePollRequestDbo> Requests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                @"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True"
            );
        }
    }
}