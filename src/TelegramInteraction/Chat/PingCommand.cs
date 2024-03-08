using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class PingCommand : IChatCommand
    {
        public PingCommand(ITelegramBotClient telegramBotClient)
        {
            this.telegramBotClient = telegramBotClient;
        }

        public bool CanHandle(Update update) => update?.Message?.Text != null && update.Message.Text.Contains("/ping");

        public async Task ExecuteAsync(Update update)
        {
            var message = update.Message;
            await telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "pong"
            );
        }

        private readonly ITelegramBotClient telegramBotClient;
    }
}