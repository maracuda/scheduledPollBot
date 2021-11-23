using System;
using System.Threading.Tasks;

using BusinessLogic;

using SimpleInjector;

using Telegram.Bot;

using Vostok.Applications.Scheduled;
using Vostok.Hosting.Abstractions;

namespace TelegramInteraction
{
    public class ScheduledApplication : VostokScheduledAsyncApplication
    {
        protected override async Task SetupAsync(IScheduledActionsBuilder builder, IVostokHostingEnvironment environment
        )
        {
            var container = environment.HostExtensions.Get<Container>();
            
            var telegramBotClient = container.GetInstance<ITelegramBotClient>();
            var sportGroupRepository = container.GetInstance<ISportGroupRepository>();
            
            var groups = await sportGroupRepository.ReadAllAsync();
            foreach(var sportGroup in groups)
            {
                builder.Schedule(sportGroup.Name,
                                 Scheduler.Crontab(sportGroup.NotificationSchedule),
                                 context => SendPollAsync(sportGroup, telegramBotClient, context)
                );
            }
        }

        private static Task SendPollAsync(SportGroup sportGroup, ITelegramBotClient telegramBotClient,
                                          IScheduledActionContext context
        )
        {
            var trainingTime = Scheduler.Crontab(sportGroup.TrainingSchedule).ScheduleNext(DateTimeOffset.Now);
            if(!trainingTime.HasValue)
            {
                throw new Exception("Can't define training time");
            }

            return telegramBotClient.SendPollAsync(sportGroup.TelegramChatId,
                                                   $"{sportGroup.Title} {trainingTime.Value.Date:dd.MM}",
                                                   sportGroup.PollOptions,
                                                   isAnonymous: false,
                                                   cancellationToken: context.CancellationToken
            );
            return Task.CompletedTask;
        }
    }
}