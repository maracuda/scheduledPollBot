using System;
using System.Net.Http;
using System.Threading.Tasks;

using BusinessLogic;
using BusinessLogic.CreatePolls;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using SimpleInjector;

using TelegramInteraction.Chat;

using Vostok.Applications.AspNetCore;
using Vostok.Applications.AspNetCore.Builders;
using Vostok.Applications.Scheduled;
using Vostok.Hosting.Abstractions;
using Vostok.Hosting.Abstractions.Composite;
using Vostok.Hosting.Abstractions.Requirements;
using Vostok.Logging.Abstractions;

namespace TelegramInteraction
{
    [RequiresPort]
    public class MultiApplication : CompositeApplication
    {
        public MultiApplication()
            : base(builder => builder
                              .AddAspNetCore(SetupAspNetCore)
                              .AddApplication(new ScheduledApplication())
                              .AddApplication(new ChatApplication())
                              .AddScheduled(SetupPinger)
                              .UseParallelInitialization()
            )
        {
        }

        private static void SetupPinger(IScheduledActionsBuilder obj, IVostokHostingEnvironment environment)
        {
            obj.Schedule("Ping self",
                         Scheduler.Periodical(TimeSpan.FromMinutes(10)),
                         async () =>
                             {
                                 var httpClient = new HttpClient();

                                 var response =
                                     await httpClient.GetAsync("https://kontur-sport-bot.herokuapp.com/_status/ping");

                                 environment.Log.Info($"Ping result: {response.StatusCode}");
                             }
            );
        }

        public override Task PreInitializeAsync(IVostokHostingEnvironment environment)
        {
            var container = new Container();

            var applicationSettings = ApplicationSettingsProvider.Get(environment.ApplicationIdentity.Environment);
            container.RegisterInstance(applicationSettings);
            container.ConfigureTelegramClient(applicationSettings);

            container.Register<ChatWorker>();

            container.RegisterInstance(environment.Log);

            if(environment.ApplicationIdentity.Environment == EnvironmentType.Production)
            {
                container.Register<ISportGroupRepository, HardCodedSportGroupRepository>();
            }
            else
            {
                container.Register<ISportGroupRepository, DevelopSportGroupRepository>();
            }
            
            container.Register<ICommandsRouter, CommandsRouter>();
            container.Register<ICreatePollService, CreatePollService>();

            container.Collection.Register<IChatCommand>(typeof(PingCommand).Assembly);
            
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
                                    app.Run(async context => { await context.Response.WriteAsync("I'm alive!"); });
                                }
                        );
                    }
            );
        }
    }
}