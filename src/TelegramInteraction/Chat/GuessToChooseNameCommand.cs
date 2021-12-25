using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramInteraction.Chat
{
    public class GuessToChooseNameCommand : IChatCommand
    {
        private readonly ITelegramBotClient telegramBotClient;

        public GuessToChooseNameCommand(ITelegramBotClient telegramBotClient)
        {
            this.telegramBotClient = telegramBotClient;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            await telegramBotClient.SendTextMessageAsync(chatId, ChatConstants.GuessNameText, replyMarkup:new ForceReplyMarkup());
        }

        public bool CanHandle(Update update) =>
            update.CallbackQuery != null && update.CallbackQuery.Data == ChatConstants.NameCallback;
    }
}