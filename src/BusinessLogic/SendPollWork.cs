using System;
using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;

namespace BusinessLogic
{
    public class SendPollWork
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly IApplicationSettings applicationSettings;

        public SendPollWork(
            ITelegramBotClient telegramBotClient,
            IApplicationSettings applicationSettings
            )
        {
            this.telegramBotClient = telegramBotClient;
            this.applicationSettings = applicationSettings;
        }

        public async Task ExecuteAsync(CancellationToken token)
        {
            await telegramBotClient.SendPollAsync(applicationSettings.GetString("ChatId"),
                                                  $"Йога {DateTime.Now.AddDays(1).ToShortDateString()}",
                                                  new[] {"Пойду в зал", "Пойду онлайн", "Не пойду", "Возможно",},
                                                  isAnonymous: false,
                                                  cancellationToken: token
            );
        }
    }
}