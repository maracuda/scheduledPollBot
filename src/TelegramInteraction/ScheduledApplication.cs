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
        protected override async Task SetupAsync(IScheduledActionsBuilder builder, IVostokHostingEnvironment environment
        )
        {
            sendPollTasks = new ConcurrentDictionary<Guid, Task>();
            var container = environment.HostExtensions.Get<Container>();

            var telegramBotClient = container.GetInstance<ITelegramBotClient>();
            var scheduledPollService = container.GetInstance<IScheduledPollService>();
            var telegramLogger = container.GetInstance<ITelegramLogger>();
            var log = container.GetInstance<ILog>();

            builder.Schedule("Poll sending scheduler",
                             Scheduler.Periodical(2.Seconds()),
                             context => ScheduleTasks(scheduledPollService,
                                                      telegramBotClient,
                                                      context,
                                                      log,
                                                      telegramLogger
                             )
            );
        }

        private async Task ScheduleTasks(IScheduledPollService scheduledPollService,
                                         ITelegramBotClient telegramBotClient,
                                         IScheduledActionContext context, ILog log, ITelegramLogger telegramLogger
        )
        {
            var enabledPolls = (await scheduledPollService.FindNotDisabledAsync()).ToArray();
            log.Warn($"***Enabled polls: {string.Join(",", enabledPolls.Select(p => p.Name))}");
            log.Warn($"***Tasks in dict: {string.Join(",", sendPollTasks.Keys)}");

            foreach(var scheduledPoll in enabledPolls)
            {
                if(sendPollTasks.ContainsKey(scheduledPoll.Id))
                {
                    continue;
                }

                var now = DateTime.Now;
                // 10 секунд должны быть больше чем время запуска шедулера, сейчас это 2 секунды
                if(CrontabSchedule.Parse(scheduledPoll.Schedule).GetNextOccurrence(now) - now < 10.Seconds())
                {
                    sendPollTasks[scheduledPoll.Id] =
                        Task.Run(() => SendPollAsync(telegramBotClient,
                                                     context,
                                                     scheduledPoll,
                                                     log
                                 )
                            )
                            .ContinueWith(async t =>
                                              {
                                                  log.Error(t.Exception);
                                                  await telegramBotClient
                                                      .SendTextMessageAsync(
                                                          scheduledPoll.ChatId,
                                                          $"Sorry, can't send poll because of error, developers are know it and will contact you"
                                                      );
                                                  telegramLogger.Log(t.Exception);
                                              },
                                          TaskContinuationOptions.OnlyOnFaulted
                            );
                }
            }

            var disabledPollIds = sendPollTasks.Keys.Except(enabledPolls.Select(p => p.Id));
            foreach(var pollId in disabledPollIds)
            {
                sendPollTasks.Remove(pollId, out _);
                log.Warn($"***Poll {pollId} was removed");
            }
        }

        private async Task SendPollAsync(ITelegramBotClient telegramBotClient,
                                         IScheduledActionContext context, ScheduledPoll scheduledPoll, ILog log
        )
        {
            try
            {
                var contextLog = log.ForContext(scheduledPoll.Name);

                var nextOccurrence = CrontabSchedule.Parse(scheduledPoll.Schedule).GetNextOccurrence(DateTime.Now);

                var timeToSleep = nextOccurrence - DateTime.Now;
                contextLog.Warn($"***Scheduled to {nextOccurrence}, sleep for {timeToSleep}");
                await Task.Delay(timeToSleep);
                contextLog.Warn($"***Wake up");

                contextLog.Warn($"***Dict key: {string.Join(",", sendPollTasks.Keys)}");
                if(sendPollTasks.ContainsKey(scheduledPoll.Id))
                {
                    contextLog.Warn($"*** Before poll sendings");
                    await telegramBotClient.SendPollAsync(scheduledPoll.ChatId,
                                                          scheduledPoll.Name,
                                                          scheduledPoll.Options,
                                                          isAnonymous: scheduledPoll.IsAnonymous,
                                                          cancellationToken: context.CancellationToken
                    );
                    contextLog.Warn("***Message was sent");
                }
                else
                {
                    contextLog.Warn("***Sending was cancelled");
                }
            }
            finally
            {
                sendPollTasks.Remove(scheduledPoll.Id, out _);
            }
        }

        private ConcurrentDictionary<Guid, Task> sendPollTasks;
    }
}