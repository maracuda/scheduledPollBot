using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
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
                                                       "AgACAgIAAxkBAAIDTmG2FGWl43P3RENjZF6bQZTLrAwOAAJktzEbAmmwSd2POqDMKGYfAQADAgADcwADIwQ"
                                                   )
            );

            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         @"It will be send by period, which you have specified"
            );
            await telegramBotClient.SendPhotoAsync(update.Message.Chat.Id,
                                                   new InputOnlineFile(
                                                       "AgACAgIAAxkBAAIDT2G2FKVDT2Eouunzh_mOy87KfKXaAAJltzEbAmmwSfuDbRGh6bIqAQADAgADbQADIwQ"
                                                   )
            );

            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "To create poll type /new");
        }

        public bool CanHandle(Update update) => update.Message != null && update.Message.Text.StartsWith("/start");
        private readonly ITelegramBotClient telegramBotClient;
    }
}