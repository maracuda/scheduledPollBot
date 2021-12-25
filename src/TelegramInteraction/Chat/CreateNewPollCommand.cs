using System;
using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramInteraction.Chat
{
    public class CreateNewPollCommand : IChatCommand
    {
        private readonly PollSender pollSender;
        private readonly ICreatePollService createPollService;
        private readonly ITelegramBotClient telegramBotClient;

        public CreateNewPollCommand(PollSender pollSender, ICreatePollService createPollService, ITelegramBotClient telegramBotClient)
        {
            this.pollSender = pollSender;
            this.createPollService = createPollService;
            this.telegramBotClient = telegramBotClient;
        }

        public async Task ExecuteAsync(Update update)
        {
            if(update.Message.Chat.Type != ChatType.Private)
            {
                await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                             "This command available only in private chat with me"
                );
                return;
            }
            
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
                    IsValid = false,
                };
            await createPollService.CreateAsync(createPollRequest);

            await pollSender.SendPollAsync(createPollRequest);
        }

        public bool CanHandle(Update update) =>
            update?.Message?.Text != null && update.Message.Text.Contains("/new");
    }
}