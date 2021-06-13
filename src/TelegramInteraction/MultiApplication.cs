using System.Threading.Tasks;

using BusinessLogic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using SimpleInjector;

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

        
        public override Task PreInitializeAsync(IVostokHostingEnvironment environment)
        {
            var container = new Container();

            var applicationSettings = ApplicationSettingsProvider.Get();
            container.RegisterInstance(applicationSettings);
            container.ConfigureTelegramClient(applicationSettings);
            container.Register<SendPollWork>();
 
            environment.HostExtensions.AsMutable().Add(container);
 
            return Task.CompletedTask;
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
                                    app.Run(async context =>
                                        {
                                            await context.Response.WriteAsync("I'm alive!");
                                        });
                                }
                        );
                    }
            );
        }
    }

    public class Settings
    {
    }
}