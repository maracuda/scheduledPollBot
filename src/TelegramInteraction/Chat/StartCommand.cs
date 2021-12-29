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
                                                         "Hello!\r\nI can help you to schedule polls like this"
            );
            await telegramBotClient.SendPhotoAsync(update.Message.Chat.Id,
                                                   new InputOnlineFile(
                                                       "AgACAgIAAxkBAANQYcdcL6r8D8f5WMTU4Q7NHRRafUcAAi26MRtMLkBKog710sE10ccBAAMCAANtAAMjBA"
                                                   )
            );

            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         "It will be send by schedule you specified\r\n"
                                                         + "Examples:\r\n"
                                                         + "1\\. at 10pm on mondays\r\n"
                                                         + "2\\. every 5 minutes\r\n"
                                                         + "3\\. at 4am every day",
                parseMode:ParseMode.MarkdownV2
            );
            await telegramBotClient.SendPhotoAsync(update.Message.Chat.Id,
                                                   new InputOnlineFile(
                                                       "AgACAgIAAxkBAANSYcdcbVnT8K_ipg3q7EuUKGI8zY0AAi66MRtMLkBKmiSOO698IhwBAAMCAANtAAMjBA"
                                                   )
            );

            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "To create poll type /new");
        }

        public bool CanHandle(Update update) => update?.Message?.Text != null && update.Message.Text.StartsWith("/start");
        private readonly ITelegramBotClient telegramBotClient;
    }
}