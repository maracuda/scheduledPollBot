using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat;

public class GetUserCommand : IChatCommand
{
    public GetUserCommand(ITelegramBotClient telegramBotClient)
    {
        this.telegramBotClient = telegramBotClient;
    }

    public bool CanHandle(Update update) =>
        update?.Message?.Text != null && update.Message.Text.Contains("/userId");

    public async Task ExecuteAsync(Update update)
    {
        var userId = long.Parse(update.Message.Text.Split(" ")[1]);

        await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"tg://user?id={userId}");
    }

    private readonly ITelegramBotClient telegramBotClient;
}