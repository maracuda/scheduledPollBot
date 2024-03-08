using System.Threading.Tasks;

using TelegramInteraction;

using Vostok.Hosting;
using Vostok.Hosting.Houston;
using Vostok.Hosting.Houston.Abstractions;
using Vostok.Hosting.Setup;

[assembly: HoustonEntryPoint(typeof(MultiApplication))]

namespace TelegramInteraction
{
    public class EntryPoint
    {
        public static async Task Main()
        {
            var host = new HoustonHost(builder =>
                    {
                        builder
                            .Everywhere
                            .SetupEnvironment(e => e.SetupApplicationIdentity(identityBuilder => identityBuilder
                                                                                  .SetProject("Bis_team")
                                                                                  .SetApplication("PollScheduler")
                                                                                  .SetEnvironment("cloud")
                                                    )
                                                    .SetupLog(l => l.SetupConsoleLog())
                            );
                    }
            );

            await host.WithConsoleCancellation().RunAsync();
        }
    }
}