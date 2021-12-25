using Microsoft.EntityFrameworkCore;

using Npgsql;

namespace BusinessLogic.CreatePolls
{
    public class PollsContext : DbContext
    {
        public PollsContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<CreatePollRequestDbo> Requests { get; set; }
        public DbSet<ScheduledPollDbo> Polls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql();
        }

        private readonly string connectionString;
    }
}