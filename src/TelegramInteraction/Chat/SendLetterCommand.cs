using System;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramInteraction.Chat;

public class SendLetterCommand : IChatCommand
{
    public SendLetterCommand(
        ITelegramBotClient telegramBotClient
    )
    {
        this.telegramBotClient = telegramBotClient;
    }

    public async Task ExecuteAsync(Update update)
    {
        var chatId = -long.Parse(update.Message.Text.Remove(0, "/sendLetter".Length));

        try
        {
            await telegramBotClient.SendTextMessageAsync(chatId,
                                                         @"Привет! Удобно получать опросы по расписанию, правда?😃
С понедельника бот сможет присылать опросы только для групп, которые оплатили пользование. Увы, но бесплатно бот работать не может.
Цена всего 99 рублей в месяц или 999 рублей в год. Подумайте, сколько стоит ваше время ;)

Переводить по номеру на Тинькофф +79126946867 или [ссылке](https://www.tinkoff.ru/cf/8b90vKEAlFp), в сообщении укажите название группы.
Для связи используйте команду /feedback",
                                                         ParseMode.Markdown
            );
        }
        catch(Exception ex)
        {
            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         $"Не могу отправить письмо в чат {chatId}, ошибка:\r\n {ex.Message}"
            );
        }

        var chat = await telegramBotClient.GetChatAsync(chatId);
        await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                     $"Письмо отправлено в чат: {chat.Title}"
        );
    }

    public bool CanHandle(Update update) =>
        update.Message?.Text != null
        && update.Message.Text.StartsWith("/sendLetter");

    private readonly ITelegramBotClient telegramBotClient;
}