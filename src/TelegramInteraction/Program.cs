using System;
using System.Threading.Tasks;

using Vostok.Hosting;
using Vostok.Hosting.Setup;
using Vostok.Logging.Abstractions;

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
                    .SetupLog(logBuilder => logBuilder.SetupConsoleLog().CustomizeLog(l => l.WithEventsDroppedBySourceContext("Poll sending scheduler")))
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
    }
}