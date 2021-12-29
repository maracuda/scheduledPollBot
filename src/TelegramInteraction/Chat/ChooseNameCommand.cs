using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class ChooseNameCommand : IChatCommand
    {
        public ChooseNameCommand(ICreatePollService createPollService,
                                 PollSender pollSender,
                                 ITelegramBotClient telegramBotClient,
                                 ChooseNameValidator chooseNameValidator
        )
        {
            this.createPollService = createPollService;
            this.pollSender = pollSender;
            this.telegramBotClient = telegramBotClient;
            this.chooseNameValidator = chooseNameValidator;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.Message.Chat.Id;

            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.Message.From.Id);
            if(pendingRequest == null)
            {
                await telegramBotClient.SendTextMessageAsync(chatId,
                                                             $"Sorry, there is no poll from you, to create one use /new"
                );
                return;
            }

            var validationResult = chooseNameValidator.Validate(update.Message.Text);
            if(validationResult.IsSuccess)
            {
                pendingRequest.PollName = update.Message.Text;
                await createPollService.SaveAsync(pendingRequest);
            }
            else
            {
                await telegramBotClient.SendTextMessageAsync(chatId, validationResult.Error);
            }

            await pollSender.SendPollAsync(pendingRequest);
        }

        public bool CanHandle(Update update) =>
            update.Message?.ReplyToMessage != null && update.Message.ReplyToMessage.Text == ChatConstants.GuessNameText;

        private readonly ICreatePollService createPollService;
        private readonly PollSender pollSender;
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ChooseNameValidator chooseNameValidator;
    }
}