using System;
using System.Linq;
using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class AddPollOptionCommand : IChatCommand
    {
        public AddPollOptionCommand(ITelegramBotClient telegramBotClient, ICreatePollService createPollService)
        {
            this.telegramBotClient = telegramBotClient;
            this.createPollService = createPollService;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.Message.Chat.Id;
            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.Message.From.Id);

            var pollOptions = update.Message.Text.Split(
                                                            new[] { "\r\n", "\r", "\n" },
                                                            StringSplitOptions.None);

            pendingRequest.Options = pollOptions;
            await createPollService.SaveAsync(pendingRequest);

            await telegramBotClient.SendTextMessageAsync(chatId, "Ok here is your poll");
            await telegramBotClient.SendPollAsync(chatId, pendingRequest.PollName, pollOptions, isAnonymous: pendingRequest.IsAnonymous);
        }

        public bool CanHandle(Update update) =>
            update.Message != null
            && update.Message.ReplyToMessage != null
            && update.Message.ReplyToMessage.Text.Contains("Please add options");

        private readonly ITelegramBotClient telegramBotClient;
        private readonly ICreatePollService createPollService;
    }
}