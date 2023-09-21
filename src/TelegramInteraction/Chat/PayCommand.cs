using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramInteraction.Chat;

public class PayCommand : IChatCommand
{
    public PayCommand(
        ITelegramBotClient telegramBotClient
    )
    {
        this.telegramBotClient = telegramBotClient;
    }

    public async Task ExecuteAsync(Update update)
    {
        await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                     @"Привет! Удобно получать опросы по расписанию, правда?😃
Цена всего 99 рублей в месяц или 999 рублей в год. Подумайте, сколько стоит ваше время ;)

Переводить по номеру на Тинькофф +79126946867 или [ссылке](https://www.tinkoff.ru/cf/8b90vKEAlFp), в сообщении укажите название группы.
Для связи используйте команду /feedback",
                                                     ParseMode.Markdown
        );
    }

    public bool CanHandle(Update update) => update?.Message?.Text != null && update.Message.Text.StartsWith("/pay");

    private readonly ITelegramBotClient telegramBotClient;
}