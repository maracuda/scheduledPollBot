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

        public string[] SupportedTemplates => new[] {"/ping"};

        public async Task ExecuteAsync(Message message)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "pong"
            );
        }

        private readonly ITelegramBotClient telegramBotClient;
    }
}