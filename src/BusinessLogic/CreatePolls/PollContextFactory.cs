using Microsoft.EntityFrameworkCore.Design;

namespace BusinessLogic.CreatePolls
{
    public class PollContextFactory : IPollContextFactory, IDesignTimeDbContextFactory<PollsContext>
    {
        private readonly IApplicationSettings applicationSettings;

        public PollContextFactory(IApplicationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }

        public PollsContext Create()
        {
            return CreateDbContext(new []{"a"});
        }

        public PollsContext CreateDbContext(string[] args)
        {
            var localApplicationSettings = applicationSettings.GetString("DATABASE_URL");
            return new PollsContext(localApplicationSettings);
        }
    }
}