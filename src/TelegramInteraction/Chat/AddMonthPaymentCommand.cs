using System;
using System.Threading.Tasks;

using BusinessLogic;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat;

public class AddMonthPaymentCommand : IChatCommand
{
    public AddMonthPaymentCommand(
        ITelegramBotClient telegramBotClient,
        IPaymentsRepository paymentsRepository
    )
    {
        this.telegramBotClient = telegramBotClient;
        this.paymentsRepository = paymentsRepository;
    }

    public async Task ExecuteAsync(Update update)
    {
        var chatId = -long.Parse(update.Message.Text.Remove(0, "/payforMonth".Length));

        await paymentsRepository.CreateAsync(new PaymentDbo()
                {
                    ChatId = chatId,
                    Id = Guid.NewGuid(),
                    Amount = 99,
                    Date = DateTime.UtcNow,
                    Period = PeriodType.Month,
                    PeriodsCount = 1,
                }
        );

        var chat = await telegramBotClient.GetChatAsync(chatId);
        await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                     $"Чат {chat.Title} оплачен на месяц"
        );
    }

    public bool CanHandle(Update update) =>
        update.Message?.Text != null
        && update.Message.Text.StartsWith("/payforMonth");

    private readonly ITelegramBotClient telegramBotClient;
    private readonly IPaymentsRepository paymentsRepository;
}