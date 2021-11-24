using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramInteraction.Chat
{
    public class ChooseNameCommand : IChatCommand
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ICreatePollService createPollService;

        public ChooseNameCommand(ITelegramBotClient telegramBotClient, ICreatePollService createPollService)
        {
            this.telegramBotClient = telegramBotClient;
            this.createPollService = createPollService;
        }

        public async Task ExecuteAsync(Message message)
        {
            var pendingRequest = await createPollService.FindPendingAsync(message.Chat.Id, message.From.Id);

            var name = message.Text;

            pendingRequest.PollName = name;
            await createPollService.SaveAsync(pendingRequest);
            
            await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Ok, name is {name}");
            
            await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Choose poll type",
                replyMarkup:new InlineKeyboardMarkup(new []
                    {
                        new InlineKeyboardButton
                            {
                                Text = "Anonymous",
                                CallbackData = "anonymous",
                            },
                        new InlineKeyboardButton
                            {
                                Text = "Non-Anonymous",
                                CallbackData = "nonAnonymous",
                            },
                    })
                );
        }

        public bool CanHandle(Message message
        ) =>
            message.ReplyToMessage != null
            && message.ReplyToMessage.Text.Contains("Choose name");
    }
}