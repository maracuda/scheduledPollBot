using System.Net;
using System.Net.Http;

using BusinessLogic;

using SimpleInjector;

using Telegram.Bot;

namespace TelegramInteraction
{
    public static class TelegramClientDependencyConfigurator
    {
        public static void ConfigureTelegramClient(this Container container, IApplicationSettings settings
        )
        {
            var token = settings.GetString("BotToken");
            var telegramBotClient = new TelegramBotClient(token, new HttpClient(new HttpClientHandler()
                {
                    Proxy = new WebProxy("http://proxy-external-generic.dev.kontur.ru:3128"),
                }));

            container.RegisterInstance<ITelegramBotClient>(telegramBotClient);
        }
    }
}