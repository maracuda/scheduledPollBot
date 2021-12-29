using System;
using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using FluentAssertions;
using FluentAssertions.Execution;

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
                await telegramBotClient.SendTextMessageAsync(chatId,
                                                             $"Sorry, there is no poll from you, to create one use /new"
                );
                return;
            }

            var pollOptions = update.Message.Text.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            var validationResult = Validate(pollOptions);
            if(validationResult.IsSuccess)
            {
                pendingRequest.Options = pollOptions;
                await createPollService.SaveAsync(pendingRequest);
            }
            else
            {
                await telegramBotClient.SendTextMessageAsync(chatId, validationResult.Error);
            }

            await pollSender.SendPollAsync(pendingRequest);
        }

        private static Result<string> Validate(string[] pollOptions)
        {
            try
            {
                pollOptions.Length.Should().BeGreaterOrEqualTo(2);
                pollOptions.Should().OnlyContain(o => !string.IsNullOrEmpty(o));
                pollOptions.Should().OnlyContain(o => o.Length <= 100);

                return Result<string>.Ok();
            }
            catch(AssertionFailedException exception)
            {
                return Result<string>.Fail(exception.Message);
            }
        }

        public bool CanHandle(Update update) =>
            update.Message?.ReplyToMessage?.Text != null
            && update.Message.ReplyToMessage.Text.StartsWith(ChatConstants.GuessOptionsText);

        private readonly ICreatePollService createPollService;
        private readonly PollSender pollSender;
        private readonly ITelegramBotClient telegramBotClient;
    }
}