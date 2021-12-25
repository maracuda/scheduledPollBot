using System.Threading.Tasks;

using Telegram.Bot.Args;

namespace TelegramInteraction.Chat
{
    public interface ICommandsRouter
    {
        Task RouteAsync(UpdateEventArgs updateEventArgs);
    }
}