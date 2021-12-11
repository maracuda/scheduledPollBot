using System;
using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using NCrontab;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class AddPollToChatCommand : IChatCommand
    {
        private readonly ICreatePollService createPollService;
        private readonly ITelegramBotClient telegramBotClient;
        private IScheduledPollService scheduledPollService;

        public AddPollToChatCommand(ICreatePollService createPollService,
                                    ITelegramBotClient telegramBotClient, IScheduledPollService scheduledPollService
        )
        {
            this.createPollService = createPollService;
            this.telegramBotClient = telegramBotClient;
            this.scheduledPollService = scheduledPollService;
        }

        public async Task ExecuteAsync(Update update)
        {
            var pendingRequest = await createPollService.FindPendingAsync(update.Message.From.Id);

            await scheduledPollService.CreateAsync(new ScheduledPoll
                    {
                        Id = Guid.NewGuid(),
                        CreatedRequestId = pendingRequest.Id,
                        Options = pendingRequest.Options,
                        IsAnonymous = pendingRequest.IsAnonymous,
                        Name = pendingRequest.PollName,
                        Schedule = pendingRequest.Schedule,
                        ChatId = update.Message.Chat.Id,
                    }
            );
            
            var nextOccurrence = CrontabSchedule.Parse(pendingRequest.Schedule).GetNextOccurrence(DateTime.Now);
            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         $"Ok, I will publish poll {pendingRequest.PollName}"
                                                         + $" at {nextOccurrence} next time"
            );
            
        }

        public bool CanHandle(Update update) =>
            update.Message != null && update.Message.Text.Contains("/addToChat");
    }
}