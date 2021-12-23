using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.CreatePolls
{
    public class PollsContext : DbContext
    {
        public PollsContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<CreatePollRequestDbo> Requests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                connectionString
            );
        }

        private readonly string connectionString;
    }
}