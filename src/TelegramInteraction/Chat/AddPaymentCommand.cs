using System;
using System.Threading.Tasks;

using BusinessLogic;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat;

public class AddPaymentCommand : IChatCommand
{
    public AddPaymentCommand(
        ITelegramBotClient telegramBotClient,
        IPaymentsRepository paymentsRepository
    )
    {
        this.telegramBotClient = telegramBotClient;
        this.paymentsRepository = paymentsRepository;
    }

    public async Task ExecuteAsync(Update update)
    {
        var chatId = -long.Parse(update.Message.Text.Remove(0, "/payforever".Length));

        await paymentsRepository.CreateAsync(new PaymentDbo()
                {
                    ChatId = chatId,
                    Id = Guid.NewGuid(),
                    Amount = 99999,
                    Date = DateTime.UtcNow,
                    Period = PeriodType.Year,
                    PeriodsCount = 100,
                }
        );

        var chat = await telegramBotClient.GetChatAsync(chatId);
        await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                     $"Чат {chat.Title} оплачен навечно"
        );
    }

    public bool CanHandle(Update update) =>
        update.Message?.Text != null
        && update.Message.Text.StartsWith("/payforever");

    private readonly ITelegramBotClient telegramBotClient;
    private readonly IPaymentsRepository paymentsRepository;
}