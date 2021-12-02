using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class ChooseNameCommand : IChatCommand
    {
        private readonly ICreatePollService createPollService;
        private readonly PollSender pollSender;

        public ChooseNameCommand(ICreatePollService createPollService,
                                 PollSender pollSender
                                 )
        {
            this.createPollService = createPollService;
            this.pollSender = pollSender;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.Message.Chat.Id;

            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.Message.From.Id);
            pendingRequest.PollName = update.Message.Text;
            await createPollService.SaveAsync(pendingRequest);
            
            await pollSender.SendPollAsync(pendingRequest);
        }

        public bool CanHandle(Update update) =>
            update.Message?.ReplyToMessage != null && update.Message.ReplyToMessage.Text == ChatConstants.GuessNameText;
    }
}