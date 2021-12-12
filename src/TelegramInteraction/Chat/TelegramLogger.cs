using System;

using BusinessLogic;

using Telegram.Bot;

namespace TelegramInteraction.Chat
{
    public class TelegramLogger : ITelegramLogger
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly IApplicationSettings applicationSettings;

        public TelegramLogger(
            ITelegramBotClient telegramBotClient,
            IApplicationSettings applicationSettings
        )
        {
            this.telegramBotClient = telegramBotClient;
            this.applicationSettings = applicationSettings;
        }

        public void Log(Exception exception)
        {
            var chatId = long.Parse(applicationSettings.GetString("AdminChatId"));
            telegramBotClient.SendTextMessageAsync(chatId, $"Error in bot\r\n{exception}");
        }
    }
}