using System.Threading.Tasks;

using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public interface IChatCommand
    {
        Task ExecuteAsync(Message message);
        string[] SupportedTemplates { get; }
    }
}