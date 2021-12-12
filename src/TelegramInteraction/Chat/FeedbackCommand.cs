using System;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class FeedbackCommand : IChatCommand
    {
        public FeedbackCommand(
            ITelegramBotClient telegramBotClient,
            ITelegramLogger telegramLogger
        )
        {
            this.telegramBotClient = telegramBotClient;
            this.telegramLogger = telegramLogger;
        }

        public async Task ExecuteAsync(Update update)
        {
            telegramLogger.Log(new Exception($"Feedback from @{update.Message.From.Username}\r\n{update.Message.Text}")
                );
            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Thank you! Have a nice day 😉");
        }

        public bool CanHandle(Update update) =>
            update.Message?.ReplyToMessage != null
            && update.Message.ReplyToMessage.Text == ChatConstants.FeedbackMessage;

        private readonly ITelegramBotClient telegramBotClient;
        private readonly ITelegramLogger telegramLogger;
    }
}