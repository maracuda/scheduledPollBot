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
using Vostok.Logging.Abstractions;

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
            var log = container.GetInstance<ILog>();

            builder.Schedule("Poll sending scheduler",
                             Scheduler.Periodical(2.Seconds()),
                             context => ScheduleTasks(scheduledPollService, telegramBotClient, context, log)
            );
        }

        private async Task ScheduleTasks(IScheduledPollService scheduledPollService,
                                         ITelegramBotClient telegramBotClient,
                                         IScheduledActionContext context, ILog log
        )
        {
            var enabledPolls = (await scheduledPollService.FindNotDisabledAsync())
                .ToArray();
            log.Info($"***Enabled polls: {string.Join(",", enabledPolls.Select(p => p.Name))}");

            foreach(var scheduledPoll in enabledPolls)
            {
                if(sendPollTasks.ContainsKey(scheduledPoll.Id))
                {
                    continue;
                }

                sendPollTasks[scheduledPoll.Id] = Task.Run(() => SendPollAsync(telegramBotClient, context, scheduledPoll, log));
            }

            var disabledPollIds = sendPollTasks.Keys.Except(enabledPolls.Select(p => p.Id));
            foreach(var pollId in disabledPollIds)
            {
                sendPollTasks.Remove(pollId, out _);
                log.Info($"***Poll {pollId} was removed");
            }
        }

        private async Task SendPollAsync(ITelegramBotClient telegramBotClient,
                                         IScheduledActionContext context, ScheduledPoll scheduledPoll, ILog log
        )
        {
            var contextLog = log.ForContext(scheduledPoll.Name);

            var nextOccurrence = CrontabSchedule.Parse(scheduledPoll.Schedule).GetNextOccurrence(DateTime.Now);

            var timeToSleep = nextOccurrence - DateTime.Now;
            
            contextLog.Info($"***Scheduled to {nextOccurrence}, sleep for {timeToSleep}");
            await Task.Delay(timeToSleep);
            contextLog.Info($"***Wake up");
            
            if(sendPollTasks.ContainsKey(scheduledPoll.Id))
            {
                await telegramBotClient.SendPollAsync(scheduledPoll.ChatId,
                                                      scheduledPoll.Name,
                                                      scheduledPoll.Options,
                                                      isAnonymous: scheduledPoll.IsAnonymous,
                                                      cancellationToken: context.CancellationToken
                );
                contextLog.Info("***Message was sent");
            }
            else
            {
                contextLog.Info("***Sending was cancelled");
            }
            
            sendPollTasks.Remove(scheduledPoll.Id, out _);
        }
    }
}