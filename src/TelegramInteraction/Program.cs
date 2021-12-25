using System;
using System.Threading.Tasks;

using BusinessLogic;

using Vostok.Hosting;
using Vostok.Hosting.Setup;

namespace TelegramInteraction
{
    public static class Program
    {
        public static async Task Main()
        {
            var application = new MultiApplication();

            var port = 9949;
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PORT")))
            {
                port = int.Parse(Environment.GetEnvironmentVariable("PORT"));
            }

            void EnvironmentSetup(IVostokHostingEnvironmentBuilder builder)
            {
                builder
                    .SetupApplicationIdentity(identityBuilder => identityBuilder
                                                                 .SetProject("MyProject")
                                                                 .SetApplication("MyApplication")
                                                                 .SetEnvironment(GetEnvironment())
                                                                 .SetInstance("first")
                                                                 )
                    .SetupLog(logBuilder => logBuilder.SetupConsoleLog())
                    .SetPort(port)
                    .DisableClusterConfig()
                    .DisableHercules()
                    .DisableZooKeeper()
                    .DisableServiceBeacon();
            }

            var hostSettings = new VostokHostSettings(application, EnvironmentSetup);

            var host = new VostokHost(hostSettings);

            await host.WithConsoleCancellation().RunAsync();
        }

        private static string GetEnvironment()
        {
            return Environment.CurrentDirectory.Contains("heroku") ? EnvironmentType.Production : EnvironmentType.Develop;
        }
    }
}