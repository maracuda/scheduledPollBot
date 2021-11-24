using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;

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

            var name = message.Text.Split(" ")[1];

            pendingRequest.PollName = name;
            await createPollService.SaveAsync(pendingRequest);
            
            await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Ok, name is {name}"
                // Создавать нужно в отдельном чате, чтобы другие из группы не видели
                
                // Создали в чате с ботом
                // Позвали в групповой чат, попросили бота постить результат в этом чате
            );
        }

        public bool CanHandle(Message message
        ) =>
            message.ReplyToMessage != null
            && message.ReplyToMessage.Text.Contains("Choose name");
    }
}