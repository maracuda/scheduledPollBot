using System.Threading.Tasks;

using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public interface IChatCommand
    {
        Task ExecuteAsync(Update update);
        bool CanHandle(Update update);
    }
}