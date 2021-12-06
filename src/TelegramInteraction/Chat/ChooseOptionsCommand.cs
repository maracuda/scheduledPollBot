using System;
using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class ChooseOptionsCommand : IChatCommand
    {
        private readonly ICreatePollService createPollService;
        private readonly PollSender pollSender;

        public ChooseOptionsCommand(ICreatePollService createPollService,
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
            
            var pollOptions = update.Message.Text.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);
            pendingRequest.Options = pollOptions;
            
            await createPollService.SaveAsync(pendingRequest);
            
            await pollSender.SendPollAsync(pendingRequest);
        }

        public bool CanHandle(Update update) =>
            update.Message?.ReplyToMessage != null && update.Message.ReplyToMessage.Text == ChatConstants.GuessOptionsText;
    }
}