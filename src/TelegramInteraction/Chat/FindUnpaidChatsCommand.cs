using System;
using System.Linq;
using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat;

public class FindUnpaidChatsCommand : IChatCommand
{
    public FindUnpaidChatsCommand(
        ITelegramBotClient telegramBotClient,
        IScheduledPollRepository scheduledPollRepository,
        ICreatePollRepository createPollRepository,
        IPaymentsRepository paymentsRepository
    )
    {
        this.telegramBotClient = telegramBotClient;
        this.scheduledPollRepository = scheduledPollRepository;
        this.createPollRepository = createPollRepository;
        this.paymentsRepository = paymentsRepository;
    }

    public async Task ExecuteAsync(Update update)
    {
        var notDisabledPolls = await scheduledPollRepository.FindNotDisabledAsync();
        var requests = await createPollRepository.ReadManyAsync(notDisabledPolls.Select(p => p.CreatedRequestId).ToArray());
        var activePaymentChatIds =
            (await paymentsRepository.SearchActiveAsync(notDisabledPolls.Select(p => p.ChatId).ToArray()))
            .Select(p => p.ChatId)
            .ToHashSet();

        var unpaidLines = string.Join("\r\n",
                                      requests.Where(r => r.CreateAt < DateTime.Now.AddDays(-60))
                                              .Where(r => !activePaymentChatIds.Contains(r.ChatId))
                                              .Select(r => $"{r.ChatId}: {(DateTime.Now - r.CreateAt).TotalDays / 30}")
        );

        await telegramBotClient.SendTextMessageAsync(update.Message!.Chat.Id, unpaidLines);
    }

    public bool CanHandle(Update update) =>
        update.Message?.Text != null
        && update.Message.Text == "/unpaids";

    private readonly ITelegramBotClient telegramBotClient;
    private readonly IScheduledPollRepository scheduledPollRepository;
    private readonly ICreatePollRepository createPollRepository;
    private readonly IPaymentsRepository paymentsRepository;
}