using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class GuessToPublishCommand : IChatCommand
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ICreatePollService createPollService;
        private readonly IPublishRequestValidator publishRequestValidator;
        private readonly PollSender pollSender;

        public GuessToPublishCommand(ITelegramBotClient telegramBotClient,
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
            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.CallbackQuery.From.Id); if(pendingRequest == null)
            {
                await telegramBotClient.SendTextMessageAsync(chatId, $"Sorry, there is no poll from you, to create one use /new");
                return;
            }

            var validationResult = publishRequestValidator.Validate(pendingRequest);
            if(validationResult.IsSuccess)
            {
                var bot = await telegramBotClient.GetMeAsync();
                
                await telegramBotClient.SendTextMessageAsync(chatId, $"Well done\r\nCall me @{bot.Username} in chat where you want to post the poll");
                return;
            }
            
            await telegramBotClient.SendTextMessageAsync(chatId, validationResult.Error);
            await pollSender.SendPollAsync(pendingRequest);
        }

        public bool CanHandle(Update update) =>
            update.CallbackQuery != null && update.CallbackQuery.Data == ChatConstants.PublishCallback;
    }
}