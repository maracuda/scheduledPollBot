using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private ConcurrentDictionary<Guid, bool> sendPollTasks;

        protected override async Task SetupAsync(IScheduledActionsBuilder builder, IVostokHostingEnvironment environment
        )
        {
            sendPollTasks = new ConcurrentDictionary<Guid, bool>();
            var container = environment.HostExtensions.Get<Container>();
            
            var telegramBotClient = container.GetInstance<ITelegramBotClient>();
            var scheduledPollService = container.GetInstance<IScheduledPollService>();

            builder.Schedule("one time scheduler",
                             Scheduler.Periodical(30.Seconds()),
                             context => ScheduleTask(scheduledPollService, telegramBotClient, context)
            );
        }

        private async Task ScheduleTask(IScheduledPollService scheduledPollService, ITelegramBotClient telegramBotClient,
                                        IScheduledActionContext context
        )
        {
            var allPolls = await scheduledPollService.ReadAllAsync();

            foreach(var scheduledPoll in allPolls)
            {
                if(sendPollTasks.ContainsKey(scheduledPoll.Id))
                {
                    return;
                }
                else
                {
                    sendPollTasks[scheduledPoll.Id] = true;
                    await SendPollAsync(telegramBotClient, context, scheduledPoll, sendPollTasks);
                }
            }
        }

        private static async Task SendPollAsync(ITelegramBotClient telegramBotClient,
                                                IScheduledActionContext context, ScheduledPoll scheduledPoll,
                                                ConcurrentDictionary<Guid, bool> sendPollTasks
        )
        {

            var nextOccurrence = CrontabSchedule.Parse(scheduledPoll.Schedule).GetNextOccurrence(DateTime.Now);
            await Task.Delay(nextOccurrence - DateTime.Now);

            await telegramBotClient.SendPollAsync(scheduledPoll.ChatId,
                                                  scheduledPoll.Name,
                                                  scheduledPoll.Options,
                                                  isAnonymous: scheduledPoll.IsAnonymous,
                                                  cancellationToken: context.CancellationToken
            );
            
            sendPollTasks.Remove(scheduledPoll.Id, out _);
        }
    }
}