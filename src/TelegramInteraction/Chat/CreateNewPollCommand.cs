using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramInteraction.Chat
{
    public class CreateNewPollCommand : IChatCommand
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ICreatePollService createPollService;

        public CreateNewPollCommand(ITelegramBotClient telegramBotClient, ICreatePollService createPollService)
        {
            this.telegramBotClient = telegramBotClient;
            this.createPollService = createPollService;
        }

        public async Task ExecuteAsync(Message message)
        {
            var requestId = await createPollService.CreateAsync(message.Chat.Id, message.From.Id);
            await telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Choose name"
                                                                          + "F.e. \"My awesome poll\"",
                                                         replyMarkup:new InlineKeyboardMarkup(new InlineKeyboardButton
                                                             {
                                                                 Text = "txt",
                                                                 CallbackData = $"/callback_addName_{requestId}",
                                                             })
                // Создавать нужно в отдельном чате, чтобы другие из группы не видели
                
                // Создали в чате с ботом
                // Позвали в групповой чат, попросили бота постить результат в этом чате
            );
        }

        public string[] SupportedTemplates => new[] { "/new" };

        public CommandType Type => CommandType.Text;
    }
}