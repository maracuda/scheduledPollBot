using System.IO;
using System.Threading.Tasks;

using BusinessLogic;

using SimpleInjector;

using TelegramInteraction.Chat;

using Vostok.Hosting.Abstractions;
using Vostok.Hosting.Abstractions.Composite;
using Vostok.Hosting.Abstractions.Requirements;

namespace TelegramInteraction
{
    [RequiresPort]
    [RequiresConfiguration(typeof(MySettings))]
    public class MultiApplication : CompositeApplication
    {
        public MultiApplication()
            : base(builder => builder
                              .AddApplication(new ScheduledApplication())
                              .AddApplication(new ChatApplication())
            )
        {
        }

        public override Task PreInitializeAsync(IVostokHostingEnvironment environment)
        {
            var container = new Container();

            var applicationSettings = GetSettings(environment);
            container.RegisterInstance(applicationSettings);
            container.ConfigureTelegramClient(applicationSettings);

            container.Register<ITelegramLogger, TelegramLogger>(Lifestyle.Singleton);

            container.Register<ChatWorker>();

            container.Register<IScheduledPollService, ScheduledPollService>(Lifestyle.Singleton);
            container.Register<IScheduledPollRepository, HardCodedPollRepository>(Lifestyle.Singleton);

            container.RegisterInstance(environment.Log);

            container.Register<ICommandsRouter, CommandsRouter>();

            container.Collection.Register<IChatCommand>(typeof(PingCommand).Assembly);

            environment.HostExtensions.AsMutable().Add(container);

            return Task.CompletedTask;
        }

        private IApplicationSettings GetSettings(IVostokHostingEnvironment vostokHostingEnvironment)
        {
            {
                if(File.Exists("Settings/settings.txt"))
                {
                    return new LocalApplicationSettings();
                }

                var mySettings = vostokHostingEnvironment.ConfigurationProvider.Get<MySettings>();
                return new HoustonApplicationSettings(mySettings);
            }
        }
    }
}