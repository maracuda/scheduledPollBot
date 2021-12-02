using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class ChooseIsAnonymousCommand : IChatCommand
    {
        public ChooseIsAnonymousCommand(ICreatePollService createPollService, PollSender pollSender)
        {
            this.createPollService = createPollService;
            this.pollSender = pollSender;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.CallbackQuery.From.Id);
            var isAnonymous = !pendingRequest.IsAnonymous;

            pendingRequest.IsAnonymous = isAnonymous;
            await createPollService.SaveAsync(pendingRequest);

            await pollSender.SendPollAsync(pendingRequest);
        }

        public bool CanHandle(Update update) =>
            update.CallbackQuery != null && update.CallbackQuery.Data == ChatConstants.AnonymousCallback;

        private readonly ICreatePollService createPollService;
        private readonly PollSender pollSender;
    }
}