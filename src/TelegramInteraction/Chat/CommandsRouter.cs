using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class CommandsRouter : ICommandsRouter
    {
        public CommandsRouter(ITelegramBotClient telegramBotClient)
        {
            this.telegramBotClient = telegramBotClient;
        }

        public async Task RouteAsync(Message message)
        {
            switch(message.Text.Split(' ').First())
            {
            case "/ping":
                await telegramBotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"pong, chatId: {message.Chat.Id}"
                );
                break;
            }
        }

        private readonly ITelegramBotClient telegramBotClient;
    }
}