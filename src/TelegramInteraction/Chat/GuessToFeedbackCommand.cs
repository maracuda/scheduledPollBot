using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramInteraction.Chat
{
    public class GuessToFeedbackCommand : IChatCommand
    {
        public GuessToFeedbackCommand(
            ITelegramBotClient telegramBotClient
        )
        {
            this.telegramBotClient = telegramBotClient;
        }

        public async Task ExecuteAsync(Update update)
        {
            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         ChatConstants.FeedbackMessage,
                                                         replyMarkup: new ForceReplyMarkup()
            );
        }

        public bool CanHandle(Update update) => update?.Message?.Text != null && update.Message.Text.StartsWith("/feedback");
        private readonly ITelegramBotClient telegramBotClient;
    }
}