using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramInteraction.Chat
{
    public class GuessToChooseOptionsCommand : IChatCommand
    {
        private readonly ITelegramBotClient telegramBotClient;

        public GuessToChooseOptionsCommand(ITelegramBotClient telegramBotClient)
        {
            this.telegramBotClient = telegramBotClient;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            await telegramBotClient.SendTextMessageAsync(chatId, ChatConstants.GuessOptionsText+"\r\nFirst\r\nSecond", replyMarkup:new ForceReplyMarkup(), parseMode:ParseMode.MarkdownV2);
        }

        public bool CanHandle(Update update) =>
            update.CallbackQuery != null && update.CallbackQuery.Data == ChatConstants.OptionsCallback;
    }
}