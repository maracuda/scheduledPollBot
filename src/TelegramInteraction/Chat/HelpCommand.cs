using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class HelpCommand : IChatCommand
    {
        public HelpCommand(ITelegramBotClient telegramBotClient)
        {
            this.telegramBotClient = telegramBotClient;
        }

        public async Task ExecuteAsync(Update update)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Привет\r\nЯ умею посылать опросы по расписанию\r\n"
                      + "Если ты хочешь себе опрос, то напиши @maracuda\r\n"
                      + "Мой код лежит тут https://github.com/maracuda/KonturSportBot, приходи и научи меня делать что-то интересное ;)"
            );
        }

        public bool CanHandle(Update update) =>
            update.Message != null && (update.Message.Text.Contains("/help") || update.Message.Text.Contains("/start"));

        private readonly ITelegramBotClient telegramBotClient;
    }
}