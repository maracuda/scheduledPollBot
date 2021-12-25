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
            
            return new PollsContext(localApplicationSettings.GetString("DATABASE_URL"));
        }
    }
}