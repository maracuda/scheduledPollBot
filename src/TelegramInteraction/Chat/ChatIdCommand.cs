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

        public string[] SupportedTemplates => new[] {"/chatId"};

        public async Task ExecuteAsync(Message message)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: message.Chat.Id.ToString()
            );
        }

        private readonly ITelegramBotClient telegramBotClient;
    }
}