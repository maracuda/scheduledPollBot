using System.Threading.Tasks;

using Vostok.Hosting;
using Vostok.Hosting.Setup;

namespace TelegramInteraction
{
    public static class Program
    {
        public static async Task Main()
        {
            var application = new ScheduledApplication();

            void EnvironmentSetup(IVostokHostingEnvironmentBuilder builder)
            {
                builder
                    .SetupApplicationIdentity(identityBuilder => identityBuilder
                                                                 .SetProject("MyProject")
                                                                 .SetApplication("MyApplication")
                                                                 .SetEnvironment("Local")
                                                                 .SetInstance("first")
                                                                 )
                    .SetupLog(logBuilder => logBuilder.SetupFileLog()
                    );
            }

            var hostSettings = new VostokHostSettings(application, EnvironmentSetup);

            var host = new VostokHost(hostSettings);

            await host.WithConsoleCancellation().RunAsync();
        }
    }
}