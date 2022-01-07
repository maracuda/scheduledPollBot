using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class ChatIdCommand : IChatCommand
    {
        public ChatIdCommand(ITelegramBotClient telegramBotClient)
        {
            this.telegramBotClient = telegramBotClient;
        }

        public bool CanHandle(Update update) => update?.Message?.Text != null && update.Message.Text.Contains("/chatId");

        public async Task ExecuteAsync(Update update)
        {
            var message = update.Message;
            await telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: message.Chat.Id.ToString()
            );
        }

        private readonly ITelegramBotClient telegramBotClient;
    }
}