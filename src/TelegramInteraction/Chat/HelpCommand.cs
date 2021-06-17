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

        public string[] SupportedTemplates => new[] {"/help", "/start"};

        public async Task ExecuteAsync(Message message)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Привет\r\nЯ умею посылать опросы по расписанию\r\n"
                      + "Если ты хочешь себе опрос, то напиши @maracuda\r\n"
                      + "Мой код лежит тут https://github.com/maracuda/KonturSportBot, приходи и научи меня делать что-то интересное ;)"
            );
        }

        private readonly ITelegramBotClient telegramBotClient;
    }
}