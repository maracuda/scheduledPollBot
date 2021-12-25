using Microsoft.EntityFrameworkCore.Design;

namespace BusinessLogic.CreatePolls
{
    public class PollContextFactory : IPollContextFactory, IDesignTimeDbContextFactory<PollsContext>
    {
        public PollsContext Create()
        {
            return CreateDbContext(new []{"a"});
        }

        public PollsContext CreateDbContext(string[] args)
        {
            var localApplicationSettings = ApplicationSettingsProvider.Get(EnvironmentType.Production);

            var databaseUrl = localApplicationSettings.GetString("DATABASE_URL");
            var p = new PostgreUrlParser().Parse(databaseUrl);

            var connectionString = $"Host={p.HostAndPort};Database={p.DatabaseName};Username={p.UserName};Password={p.Password}";
            return new PollsContext(connectionString);
        }
    }
}