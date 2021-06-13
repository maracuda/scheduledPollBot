using BusinessLogic;

using SimpleInjector;

using Telegram.Bot;

namespace TelegramInteraction
{
    public static class TelegramClientDependencyConfigurator
    {
        public static void ConfigureTelegramClient(this Container container, IApplicationSettings settings)
        {
            var telegramBotClient = new TelegramBotClient(settings.GetString("BotToken"));

            container.RegisterInstance<ITelegramBotClient>(telegramBotClient);
        }
    }
}