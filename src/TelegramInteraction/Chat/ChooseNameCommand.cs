using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramInteraction.Chat
{
    public class ChooseNameCommand : IChatCommand
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ICreatePollService createPollService;

        public ChooseNameCommand(ITelegramBotClient telegramBotClient, ICreatePollService createPollService)
        {
            this.telegramBotClient = telegramBotClient;
            this.createPollService = createPollService;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.CallbackQuery.From.Id);

            /*
            var name = update.Message.Text;

            pendingRequest.PollName = name;
            await createPollService.SaveAsync(pendingRequest);*/
        }

        public bool CanHandle(Update update) =>
            update.CallbackQuery != null && update.CallbackQuery.Data == CallbackConstants.NameCallback;
    }
}