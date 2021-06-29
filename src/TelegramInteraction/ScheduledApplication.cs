using System;
using System.Threading.Tasks;

using BusinessLogic;

using SimpleInjector;

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
            var sendPollWork = container.GetInstance<SendPollWork>();

            var sportGroupRepository = container.GetInstance<ISportGroupRepository>();
            var groups = await sportGroupRepository.ReadAllAsync();

            foreach(var sportGroup in groups)
            {
                builder.Schedule(sportGroup.Name, Scheduler.Crontab(sportGroup.NotificationSchedule),
                                 context 
                                     =>
                                     {
                                         // NOTE: не хотелось тащить Scheduler.Crontab внутрь sendPollWork.ExecuteAsync
                                         // Но может это и правильно
                                         var trainingTime = Scheduler.Crontab(sportGroup.TrainingSchedule).ScheduleNext(DateTimeOffset.Now);
                                         if(!trainingTime.HasValue)
                                         {
                                             throw new Exception("Can't define training time");
                                         }
                                         
                                         return sendPollWork.ExecuteAsync(
                                             context.CancellationToken,
                                             sportGroup.TelegramChatId,
                                             trainingTime.Value,
                                             sportGroup.Title,
                                             sportGroup.PollOptions
                                         );
                                     }
                );
            }
        }
    }
}