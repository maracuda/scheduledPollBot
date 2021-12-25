using BusinessLogic;

using SimpleInjector;

using Telegram.Bot;

using Vostok.Logging.Abstractions;

namespace TelegramInteraction
{
    public static class TelegramClientDependencyConfigurator
    {
        public static void ConfigureTelegramClient(this Container container, IApplicationSettings settings,
                                                   ILog environmentLog
        )
        {
            var token = settings.GetString("BotToken");
            environmentLog.Error($"Bot token is {token}");
            var telegramBotClient = new TelegramBotClient(token);

            container.RegisterInstance<ITelegramBotClient>(telegramBotClient);
        }
    }
}