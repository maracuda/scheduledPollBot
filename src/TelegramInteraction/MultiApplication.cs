using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Vostok.Applications.AspNetCore;
using Vostok.Applications.AspNetCore.Builders;
using Vostok.Hosting.Abstractions;
using Vostok.Hosting.Abstractions.Composite;
using Vostok.Hosting.Abstractions.Requirements;

namespace TelegramInteraction
{
    [RequiresPort]
    public class MultiApplication : CompositeApplication
    {
        public MultiApplication()
            : base(builder => builder
                              .AddAspNetCore(SetupAspNetCore)
                              .AddApplication(new ScheduledApplication())
                              .UseParallelInitialization()
            )
        {
        }

        private static void SetupAspNetCore(IVostokAspNetCoreApplicationBuilder builder,
                                            IVostokHostingEnvironment environment
        )
        {
            builder.SetupWebHost(b =>
                    {
                        b.Configure(app =>
                                {
                                    app.UseRouting();
                                    app.UseVostokPingApi();
                                }
                        );
                    }
            );
        }
    }
}