using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class GetChatCommand : IChatCommand
    {
        public GetChatCommand(ITelegramBotClient telegramBotClient)
        {
            this.telegramBotClient = telegramBotClient;
        }

        public bool CanHandle(Update update) =>
            update?.Message?.Text != null && update.Message.Text.Contains("/chatNameById");

        public async Task ExecuteAsync(Update update)
        {
            var chatId = long.Parse(update.Message.Text.Split(" ")[1]);

            var chat = await telegramBotClient.GetChatAsync(new ChatId(chatId));

            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, chat.Title);
        }

        private readonly ITelegramBotClient telegramBotClient;
    }
}