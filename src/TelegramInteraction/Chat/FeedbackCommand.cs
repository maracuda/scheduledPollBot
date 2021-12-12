using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

using Vostok.Logging.Abstractions;

namespace TelegramInteraction.Chat
{
    public class FeedbackCommand : IChatCommand
    {
        public FeedbackCommand(
            ITelegramBotClient telegramBotClient,
            ILog log
        )
        {
            this.telegramBotClient = telegramBotClient;
            this.log = log;
        }

        public async Task ExecuteAsync(Update update)
        {
            log.Error("Feedback from @{from}\r\n{text}", update.Message.From.Username, update.Message.Text);
            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Thank you! Have a nice day 😉");
        }

        public bool CanHandle(Update update) =>
            update.Message?.ReplyToMessage != null && update.Message.ReplyToMessage.Text == ChatConstants.FeedbackMessage;

        private readonly ITelegramBotClient telegramBotClient;
        private readonly ILog log;
    }
}