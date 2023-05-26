using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace TelegramInteraction.Chat
{
    public class StartCommand : IChatCommand
    {
        public StartCommand(
            ITelegramBotClient telegramBotClient
        )
        {
            this.telegramBotClient = telegramBotClient;
        }

        public async Task ExecuteAsync(Update update)
        {
            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         "Привет!\r\nЯ могу отправлять опросы по расписанию"
            );
            await telegramBotClient.SendPhotoAsync(update.Message.Chat.Id,
                                                   new InputOnlineFile(
                                                       "AgACAgIAAxkBAANQYcdcL6r8D8f5WMTU4Q7NHRRafUcAAi26MRtMLkBKog710sE10ccBAAMCAANtAAMjBA"
                                                   )
            );

            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         "Опрос будет отправляться по времени, которое укажешь\r\n"
                                                         + "Например:\r\n"
                                                         + "1\\. в 10 вечера по понедельникам\r\n"
                                                         + "2\\. каждые 5 минут\r\n"
                                                         + "3\\. в 4 часа каждый день",
                parseMode:ParseMode.MarkdownV2
            );
            await telegramBotClient.SendPhotoAsync(update.Message.Chat.Id,
                                                   new InputOnlineFile(
                                                       "AgACAgIAAxkBAANSYcdcbVnT8K_ipg3q7EuUKGI8zY0AAi66MRtMLkBKmiSOO698IhwBAAMCAANtAAMjBA"
                                                   )
            );

            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         "Если что-то непонятно, то читай [инструкцию](https://vc.ru/tribuna/347133-bot-dlya-regulyarnyh-oprosov)",
                                                         ParseMode.MarkdownV2
            );

            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "To create poll type /new");
        }

        public bool CanHandle(Update update) => update?.Message?.Text != null && update.Message.Text.StartsWith("/start");
        private readonly ITelegramBotClient telegramBotClient;
    }
}