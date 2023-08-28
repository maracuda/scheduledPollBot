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
        var requests =
            await createPollRepository.ReadManyAsync(notDisabledPolls.Select(p => p.CreatedRequestId).ToArray());
        var activePaymentChatIds =
            (await paymentsRepository.SearchActiveAsync(notDisabledPolls.Select(p => p.ChatId).Distinct().ToArray()))
            .Select(p => p.ChatId)
            .ToHashSet();

        // todo положить в опросы дату старта, чтобы не тащить из запросов на создание
        var requestToChat = notDisabledPolls.ToDictionary(p => p.CreatedRequestId, p => p.ChatId);
        var activeUnpaidRequests = requests.Where(r => r.CreateAt < DateTime.Now.AddDays(-60))
                                           .DistinctBy(r => requestToChat[r.Id])
                                           .Where(r => !activePaymentChatIds.Contains(requestToChat[r.Id]))
                                           .ToArray();

        var chatNames = notDisabledPolls.Select(p => p.ChatId)
                                        .Distinct()
                                        .ToDictionary(chatId => chatId,
                                                      chatId =>
                                                          {
                                                              try
                                                              {
                                                                  return telegramBotClient.GetChatAsync(chatId)
                                                                      .GetAwaiter()
                                                                      .GetResult()
                                                                      .Title;
                                                              }
                                                              catch(Exception)
                                                              {
                                                                  return "Чат не найден";
                                                              }
                                                          }
                                        );

        var unpaidLines = string.Join("\r\n",
                                      activeUnpaidRequests
                                          .Select(
                                              r =>
                                                  $"{chatNames[requestToChat[r.Id]]}"
                                                  + $"\tMonths: {Math.Round((DateTime.Now - r.CreateAt).TotalDays / 30, 1)}"
                                                  + $"\t/disable{-requestToChat[r.Id]}"
                                                  + $"\t/payforever{-requestToChat[r.Id]}"
                                          )
        );

        unpaidLines += $"\r\nTotal: {activeUnpaidRequests.Length}";

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