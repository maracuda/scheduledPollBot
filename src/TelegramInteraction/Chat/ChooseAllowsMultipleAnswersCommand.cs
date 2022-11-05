using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat;

public class ChooseAllowsMultipleAnswersCommand : IChatCommand
{
    public ChooseAllowsMultipleAnswersCommand(ICreatePollService createPollService, PollSender pollSender, ITelegramBotClient telegramBotClient)
    {
        this.createPollService = createPollService;
        this.pollSender = pollSender;
        this.telegramBotClient = telegramBotClient;
    }

    public async Task ExecuteAsync(Update update)
    {
        var chatId = update.CallbackQuery.Message.Chat.Id;
        var pendingRequest = await createPollService.FindPendingAsync(chatId, update.CallbackQuery.From.Id);
        if(pendingRequest == null)
        {
            await telegramBotClient.SendTextMessageAsync(chatId, $"Sorry, there is no poll from you, to create one use /new, to create use /new");
            return;
        }
        var allowsMultipleAnswers = !pendingRequest.AllowsMultipleAnswers;

        pendingRequest.AllowsMultipleAnswers = allowsMultipleAnswers;
        await createPollService.SaveAsync(pendingRequest);

        await pollSender.SendPollAsync(pendingRequest);
    }

    public bool CanHandle(Update update) =>
        update.CallbackQuery != null && update.CallbackQuery.Data == ChatConstants.MultipleCallback;

    private readonly ICreatePollService createPollService;
    private readonly PollSender pollSender;
    private readonly ITelegramBotClient telegramBotClient;
}