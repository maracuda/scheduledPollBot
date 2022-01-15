using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramInteraction.Chat
{
    public class DonateCommand : IChatCommand
    {
        public DonateCommand(
            ITelegramBotClient telegramBotClient
        )
        {
            this.telegramBotClient = telegramBotClient;
        }

        public async Task ExecuteAsync(Update update)
        {
            await telegramBotClient.SendPhotoAsync(update.Message.Chat.Id,
                                                   "AgACAgIAAxkBAAIEp2HjDjoSSAi0Sgm0BqZhR-NOk9WVAAJqtjEbwfMZSxtTJ45rcxSMAQADAgADbQADIwQ",
                                                   "[You will be glad 🙂](https://pay.cloudtips.ru/p/1713b6da)",
                                                   ParseMode.MarkdownV2
            );
        }

        public bool CanHandle(Update update) =>
            update.Message?.Text != null
            && update.Message.Text == "/donate";

        private readonly ITelegramBotClient telegramBotClient;
    }
}