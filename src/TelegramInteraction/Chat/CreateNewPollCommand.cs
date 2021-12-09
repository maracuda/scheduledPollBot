using System;
using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class CreateNewPollCommand : IChatCommand
    {
        private readonly PollSender pollSender;
        private readonly ICreatePollService createPollService;

        public CreateNewPollCommand(PollSender pollSender, ICreatePollService createPollService)
        {
            this.pollSender = pollSender;
            this.createPollService = createPollService;
        }

        public async Task ExecuteAsync(Update update)
        {
            var createPollRequest = new CreatePollRequest
                {
                    Options = new[] { DefaultPollConstants.FirstOption, DefaultPollConstants.SecondOption, },
                    IsAnonymous = true,
                    PollName = DefaultPollConstants.Name,
                    UserId = update.Message.From.Id,
                    ChatId = update.Message.Chat.Id,
                    CreateAt = DateTime.Now,
                    IsPending = true,
                    Id = Guid.NewGuid(),
                };
            await createPollService.CreateAsync(createPollRequest);

            await pollSender.SendPollAsync(createPollRequest);
        }

        public bool CanHandle(Update update) =>
            update.Message != null && update.Message.Text.Contains("/new");
    }
}