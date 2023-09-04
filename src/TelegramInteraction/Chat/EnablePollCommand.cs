using System;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat;

public class EnablePollCommand : IChatCommand
{
    public EnablePollCommand(
        ITelegramBotClient telegramBotClient,
        IScheduledPollRepository scheduledPollRepository
    )
    {
        this.telegramBotClient = telegramBotClient;
        this.scheduledPollRepository = scheduledPollRepository;
    }

    public async Task ExecuteAsync(Update update)
    {
        var pollId = Guid.Parse(update.Message.Text.Remove(0, "/enable".Length));

        var poll = await scheduledPollRepository.ReadAsync(pollId);

        poll.IsDisabled = false;
        await scheduledPollRepository.SaveAsync(poll);

        await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                     $"Опрос {poll.Name} включен");
    }

    public bool CanHandle(Update update) =>
        update.Message?.Text != null
        && update.Message.Text.StartsWith("/enable");

    private readonly ITelegramBotClient telegramBotClient;
    private readonly IScheduledPollRepository scheduledPollRepository;
}