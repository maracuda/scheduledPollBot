using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions.Extensions;

using NCrontab;

using SimpleInjector;

using Telegram.Bot;

using TelegramInteraction.Chat;

using Vostok.Applications.Scheduled;
using Vostok.Hosting.Abstractions;

namespace TelegramInteraction
{
    public class ScheduledApplication : VostokScheduledAsyncApplication
    {
        private ConcurrentDictionary<Guid, Task> sendPollTasks;

        protected override async Task SetupAsync(IScheduledActionsBuilder builder, IVostokHostingEnvironment environment
        )
        {
            sendPollTasks = new ConcurrentDictionary<Guid, Task>();
            var container = environment.HostExtensions.Get<Container>();
            
            var telegramBotClient = container.GetInstance<ITelegramBotClient>();
            var scheduledPollService = container.GetInstance<IScheduledPollService>();

            builder.Schedule("one time scheduler",
                             Scheduler.Periodical(2.Seconds()),
                             context => ScheduleTask(scheduledPollService, telegramBotClient, context)
            );
        }

        private async Task ScheduleTask(IScheduledPollService scheduledPollService, ITelegramBotClient telegramBotClient,
                                        IScheduledActionContext context
        )
        {
            var allPolls = (await scheduledPollService.ReadAllAsync())
                .Where(p => !p.IsDisabled);

            foreach(var scheduledPoll in allPolls)
            {
                if(sendPollTasks.ContainsKey(scheduledPoll.Id))
                {
                    continue;
                }

                sendPollTasks[scheduledPoll.Id] = SendPollAsync(telegramBotClient, context, scheduledPoll);
            }
        }

        private async Task SendPollAsync(ITelegramBotClient telegramBotClient,
                                                IScheduledActionContext context, ScheduledPoll scheduledPoll
        )
        {

            var nextOccurrence = CrontabSchedule.Parse(scheduledPoll.Schedule).GetNextOccurrence(DateTime.Now);
            await Task.Delay(nextOccurrence - DateTime.Now);
            
            if(sendPollTasks.ContainsKey(scheduledPoll.Id))
            {
                await telegramBotClient.SendPollAsync(scheduledPoll.ChatId,
                                                      scheduledPoll.Name,
                                                      scheduledPoll.Options,
                                                      isAnonymous: scheduledPoll.IsAnonymous,
                                                      cancellationToken: context.CancellationToken
                );
            }
            
            sendPollTasks.Remove(scheduledPoll.Id, out _);
        }
    }
}