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

        public async Task ExecuteAsync(Update update)
        {
            var pendingRequest = await createPollService.FindPendingAsync(update.Message.Chat.Id, update.Message.From.Id);

            var name = update.Message.Text;

            pendingRequest.PollName = name;
            await createPollService.SaveAsync(pendingRequest);

            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"Ok, name is {name}");

            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         $"Choose poll type",
                                                         replyMarkup: new InlineKeyboardMarkup(new[]
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
                                                                 }
                                                         )
            );
        }

        public bool CanHandle(Update update) =>
            update.Message != null
            && update.Message.ReplyToMessage != null
            && update.Message.ReplyToMessage.Text.Contains("Choose name");
    }
}