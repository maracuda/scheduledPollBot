using BusinessLogic;

using SimpleInjector;

using Telegram.Bot;

namespace TelegramInteraction
{
    public static class TelegramClientDependencyConfigurator
    {
        public static void ConfigureTelegramClient(this Container container, IApplicationSettings settings)
        {
            var token = settings.GetString("BotToken");
            var telegramBotClient = new TelegramBotClient(token);

            container.RegisterInstance<ITelegramBotClient>(telegramBotClient);
        }
    }
}