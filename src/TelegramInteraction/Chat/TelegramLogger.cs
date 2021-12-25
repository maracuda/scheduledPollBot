using System;

using BusinessLogic;

using Telegram.Bot;

using Vostok.Logging.Abstractions;

namespace TelegramInteraction.Chat
{
    public class TelegramLogger : ITelegramLogger
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly IApplicationSettings applicationSettings;
        private readonly ILog log;

        public TelegramLogger(
            ITelegramBotClient telegramBotClient,
            IApplicationSettings applicationSettings,
            ILog log
        )
        {
            this.telegramBotClient = telegramBotClient;
            this.applicationSettings = applicationSettings;
            this.log = log.ForContext("ChatCommandsErrorLogger");
        }

        public void Log(Exception exception)
        {
            const int maxMessageLength = 4095;
            var chatId = long.Parse(applicationSettings.GetString("AdminChatId"));
            
            log.Error(exception);
            telegramBotClient.SendTextMessageAsync(chatId, $"Error in bot\r\n{exception}".Substring(0, Math.Min(maxMessageLength, exception.ToString().Length)));
        }
    }
}