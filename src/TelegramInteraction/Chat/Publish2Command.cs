using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class Publish2Command : IChatCommand
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ICreatePollService createPollService;
        private readonly IPublishRequestValidator publishRequestValidator;
        private readonly PollSender pollSender;

        public Publish2Command(ITelegramBotClient telegramBotClient,
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
            var chatId = 12312L;
            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.CallbackQuery.From.Id);

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
            update.InlineQuery != null;
    }
}