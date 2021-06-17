using System.Threading.Tasks;

using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public interface ICommandsRouter
    {
        Task RouteAsync(Message message);
    }
}