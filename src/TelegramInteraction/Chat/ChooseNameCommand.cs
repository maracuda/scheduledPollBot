using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class ChooseNameCommand : IChatCommand
    {
        private readonly ICreatePollService createPollService;
        private readonly PollSender pollSender;
        private ITelegramBotClient telegramBotClient;

        public ChooseNameCommand(ICreatePollService createPollService,
                                 PollSender pollSender,
                                 ITelegramBotClient telegramBotClient
        )
        {
            this.createPollService = createPollService;
            this.pollSender = pollSender;
            this.telegramBotClient = telegramBotClient;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.Message.Chat.Id;

            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.Message.From.Id);
            if(pendingRequest == null)
            {
                await telegramBotClient.SendTextMessageAsync(chatId, $"Sorry, there is no poll from you, to create one use /new");
                return;
            }
            
            pendingRequest.PollName = update.Message.Text;
            await createPollService.SaveAsync(pendingRequest);
            
            await pollSender.SendPollAsync(pendingRequest);
        }

        public bool CanHandle(Update update) =>
            update.Message?.ReplyToMessage != null && update.Message.ReplyToMessage.Text == ChatConstants.GuessNameText;
    }
}