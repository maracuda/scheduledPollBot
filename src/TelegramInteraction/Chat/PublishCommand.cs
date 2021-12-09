using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramInteraction.Chat
{
    public class PublishCommand : IChatCommand
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ICreatePollService createPollService;
        private readonly IPublishRequestValidator publishRequestValidator;
        private readonly PollSender pollSender;

        public PublishCommand(ITelegramBotClient telegramBotClient,
                              ICreatePollService createPollService,
                              IPublishRequestValidator publishRequestValidator,
                              PollSender pollSender
        )
        {
            this.telegramBotClient = telegramBotClient;
            this.createPollService = createPollService;
            this.publishRequestValidator = publishRequestValidator;
            this.pollSender = pollSender;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.CallbackQuery.From.Id);

            var validationResult = publishRequestValidator.Validate(pendingRequest);
            if(validationResult.IsSuccess)
            {
                await telegramBotClient.SendTextMessageAsync(chatId, ChatConstants.GuessScheduleText + " in [cron](https://crontab.guru/) format", replyMarkup:new ForceReplyMarkup(),
                                                             parseMode: ParseMode.MarkdownV2);
                return;
            }
            
            await telegramBotClient.SendTextMessageAsync(chatId, validationResult.Error);
            await pollSender.SendPollAsync(pendingRequest);
        }

        public bool CanHandle(Update update) =>
            update.CallbackQuery != null && update.CallbackQuery.Data == ChatConstants.PublishCallback;
    }
}