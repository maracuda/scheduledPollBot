using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat;

public class DisablePollCommand : IChatCommand
{
    public DisablePollCommand(
        ITelegramBotClient telegramBotClient,
        IScheduledPollRepository scheduledPollRepository
    )
    {
        this.telegramBotClient = telegramBotClient;
        this.scheduledPollRepository = scheduledPollRepository;
    }

    public async Task ExecuteAsync(Update update)
    {
        var chatId = -long.Parse(update.Message.Text.Remove(0, "/disable".Length));

        var polls = await scheduledPollRepository.FindAsync(chatId);
        foreach(var poll in polls)
        {
            poll.IsDisabled = true;
        }

        await scheduledPollRepository.SaveAsync(polls);

        await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                     $"Все опросы в чате {chatId} выключены");
    }

    public bool CanHandle(Update update) =>
        update.Message?.Text != null
        && update.Message.Text.StartsWith("/disable");

    private readonly ITelegramBotClient telegramBotClient;
    private readonly IScheduledPollRepository scheduledPollRepository;
}