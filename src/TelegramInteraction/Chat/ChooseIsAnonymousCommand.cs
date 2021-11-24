using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class ChooseIsAnonymousCommand : IChatCommand
    {
        public ChooseIsAnonymousCommand(ITelegramBotClient telegramBotClient, ICreatePollService createPollService)
        {
            this.telegramBotClient = telegramBotClient;
            this.createPollService = createPollService;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.CallbackQuery.From.Id);
            var isAnonymous = DefineIsAnonymous(update);

            pendingRequest.IsAnonymous = isAnonymous;
            await createPollService.SaveAsync(pendingRequest);
            
            var anonymousString = isAnonymous ?  "anonymous" : "non-anonymous";
            await telegramBotClient.SendTextMessageAsync(chatId, $"Ok, poll is {anonymousString}");
            
            await telegramBotClient.SendTextMessageAsync(chatId, $"Please add options");
        }

        private static bool DefineIsAnonymous(Update update)
        {
            if(update.CallbackQuery.Data == "anonymous")
            {
                return true;
            }

            return false;
        }

        public bool CanHandle(Update update) =>
            update.CallbackQuery != null && update.CallbackQuery.Message.Text == "Choose poll type";

        private readonly ITelegramBotClient telegramBotClient;
        private readonly ICreatePollService createPollService;
    }
}