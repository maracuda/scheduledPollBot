using System;
using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class ChooseOptionsCommand : IChatCommand
    {
        public ChooseOptionsCommand(ICreatePollService createPollService,
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

            var pollOptions = update.Message.Text.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            if(pollOptions.Length >= 2)
            {
                pendingRequest.Options = pollOptions;
                await createPollService.SaveAsync(pendingRequest);
            }
            else
            {
                await telegramBotClient.SendTextMessageAsync(chatId, "Please specify 2 or more options");
            }

            await pollSender.SendPollAsync(pendingRequest);
        }

        public bool CanHandle(Update update) =>
            update.Message?.ReplyToMessage != null
            && update.Message.ReplyToMessage.Text == ChatConstants.GuessOptionsText;

        private readonly ICreatePollService createPollService;
        private readonly PollSender pollSender;
        private readonly ITelegramBotClient telegramBotClient;
    }
}