using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramInteraction.Chat
{
    public class GuessToChooseScheduleCommand : IChatCommand
    {
        private readonly ITelegramBotClient telegramBotClient;

        public GuessToChooseScheduleCommand(ITelegramBotClient telegramBotClient)
        {
            this.telegramBotClient = telegramBotClient;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;

            await telegramBotClient.SendTextMessageAsync(chatId, ChatConstants.GuessScheduleText, replyMarkup:new ForceReplyMarkup());
        }

        public bool CanHandle(Update update) =>
            update.CallbackQuery != null && update.CallbackQuery.Data == ChatConstants.ScheduleCallback;
    }
}