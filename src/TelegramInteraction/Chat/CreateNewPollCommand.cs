using System;
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

        public async Task ExecuteAsync(Update update)
        {
            await createPollService.CreateAsync(update.Message.Chat.Id, update.Message.From.Id);

            await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                         "Choose name"
                                                         + Environment.NewLine
                                                         + "example: \"Training on Saturday\"<icon>",
                                                         replyMarkup: new ForceReplyMarkup()
                
                // Создавать нужно в отдельном чате, чтобы другие из группы не видели

                // Создали в чате с ботом
                // Позвали в групповой чат, попросили бота постить результат в этом чате
            );
        }

        public bool CanHandle(Update update) =>
            update.Message != null && update.Message.Text.Contains("/new");
    }
}