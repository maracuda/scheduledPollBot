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
        protected override Task SetupAsync(IScheduledActionsBuilder builder, IVostokHostingEnvironment environment
        )
        {
            sendPollTasks = new ConcurrentDictionary<Guid, Task>();
            var container = environment.HostExtensions.Get<Container>();

            var telegramBotClient = container.GetInstance<ITelegramBotClient>();
            scheduledPollService = container.GetInstance<IScheduledPollService>();
            var telegramLogger = container.GetInstance<ITelegramLogger>();
            var log = container.GetInstance<ILog>();

            builder.Schedule("Poll sending scheduler",
                             Scheduler.Periodical(2.Seconds()),
                             context => ScheduleTasks(telegramBotClient,
                                                      context,
                                                      log,
                                                      telegramLogger
                             )
            );
            return Task.CompletedTask;
        }

        private async Task ScheduleTasks(ITelegramBotClient telegramBotClient,
                                         IScheduledActionContext context, ILog log, ITelegramLogger telegramLogger
        )
        {
            var enabledPolls = (await scheduledPollService.FindNotDisabledAsync()).ToArray();

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
                                                     log,
                                                     telegramLogger
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
                                                  telegramLogger.Log(
                                                      new Exception($"poll id is: {scheduledPoll.Id}", t.Exception)
                                                  );
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
                                         IScheduledActionContext context, ScheduledPoll scheduledPoll, ILog log,
                                         ITelegramLogger telegramLogger
        )
        {
            try
            {
                var contextLog = log.ForContext(scheduledPoll.Name);

                var nextOccurrence = CrontabSchedule.Parse(scheduledPoll.Schedule).GetNextOccurrence(DateTime.Now);

                var timeToSleep = nextOccurrence - DateTime.Now;
                await Task.Delay(timeToSleep);

                if(sendPollTasks.ContainsKey(scheduledPoll.Id))
                {
                    await telegramBotClient.SendPollAsync(scheduledPoll.ChatId,
                                                          scheduledPoll.Name,
                                                          scheduledPoll.Options,
                                                          isAnonymous: scheduledPoll.IsAnonymous,
                                                          cancellationToken: context.CancellationToken,
                                                          allowsMultipleAnswers: scheduledPoll.AllowsMultipleAnswers
                    );
                    contextLog.Warn("***Message was sent");
                }
                else
                {
                    contextLog.Warn("***Sending was cancelled");
                }
            }
            catch(Exception ex)
            {
                telegramLogger.Log(new Exception($"poll id is: {scheduledPoll.Id}", ex));
            }
            finally
            {
                sendPollTasks.Remove(scheduledPoll.Id, out _);
            }
        }

        private ConcurrentDictionary<Guid, Task> sendPollTasks;
        private IScheduledPollService scheduledPollService;
    }
}