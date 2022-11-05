using System;
using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using NCrontab;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramInteraction.Chat
{
    public class ScheduleToGroupCommand : IChatCommand
    {
        public ScheduleToGroupCommand(ICreatePollService createPollService,
                                    ITelegramBotClient telegramBotClient, IScheduledPollService scheduledPollService
        )
        {
            this.createPollService = createPollService;
            this.telegramBotClient = telegramBotClient;
            this.scheduledPollService = scheduledPollService;
        }

        public async Task ExecuteAsync(Update update)
        {
            if(update.Message.Chat.Type == ChatType.Private)
            {
                await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                       "This command available only in group"
                );
                return;
            }
            
            var pendingRequest = await createPollService.FindPendingAndValidAsync(update.Message.From.Id);
            if(pendingRequest == null)
            {
                await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                             $"Sorry, there is no pending valid poll request, that i can add to this chat."
                                                             + $"\r\n Create new one with /new command in private chat with me"
                );
                return;
            }

            await scheduledPollService.CreateAsync(new ScheduledPoll
                    {
                        Id = Guid.NewGuid(),
                        CreatedRequestId = pendingRequest.Id,
                        Options = pendingRequest.Options,
                        IsAnonymous = pendingRequest.IsAnonymous,
                        Name = pendingRequest.PollName,
                        Schedule = pendingRequest.Schedule,
                        ChatId = update.Message.Chat.Id,
                        AllowsMultipleAnswers = pendingRequest.AllowsMultipleAnswers,
                    }
            );

            pendingRequest.IsPending = false;
            await createPollService.SaveAsync(pendingRequest);

            var nextOccurrence = CrontabSchedule.Parse(pendingRequest.Schedule)
                                                .GetNextOccurrence(DateTime.Now)
                                                .ToString()
                                                .Replace(".", "\\.");
            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         $"Ok, I will publish poll *{pendingRequest.PollName}*"
                                                         + $" at {nextOccurrence} *UTC* next time",
                                                         ParseMode.MarkdownV2
            );
        }

        public bool CanHandle(Update update) =>
            update?.Message?.Text != null
            && update.Message.Text.Contains("/schedule");

        private readonly ICreatePollService createPollService;
        private readonly ITelegramBotClient telegramBotClient;
        private readonly IScheduledPollService scheduledPollService;
    }
}