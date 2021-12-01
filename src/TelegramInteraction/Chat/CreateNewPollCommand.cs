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
            var createPollRequest = new CreatePollRequest
                {
                    Options = new[] { "First option", "Second option", },
                    IsAnonymous = true,
                    PollName = "Choose poll name",
                    UserId = update.Message.From.Id,
                    ChatId = update.Message.Chat.Id,
                    CreateAt = DateTime.Now,
                    IsPending = true,
                    Id = Guid.NewGuid(),
                };
            await createPollService.CreateAsync(createPollRequest);

            await telegramBotClient.SendPollAsync(createPollRequest.ChatId,
                                                  createPollRequest.PollName,
                                                  createPollRequest.Options,
                                                  isAnonymous: createPollRequest.IsAnonymous,
                                                  replyMarkup: new InlineKeyboardMarkup(
                                                      new[]
                                                          {
                                                              new[]
                                                                  {
                                                                      new InlineKeyboardButton
                                                                          {
                                                                              Text = "Name",
                                                                              CallbackData = "change_name",
                                                                          },
                                                                      new InlineKeyboardButton
                                                                          {
                                                                              Text = "Anonymous",
                                                                              CallbackData =
                                                                                  "change_anonymous",
                                                                          },
                                                                  },
                                                              new[]
                                                                  {
                                                                      new InlineKeyboardButton
                                                                          {
                                                                              Text = "Options",
                                                                              CallbackData =
                                                                                  "change_options",
                                                                          },
                                                                  },
                                                              new[]
                                                                  {
                                                                      new InlineKeyboardButton
                                                                          {
                                                                              Text = "Finish",
                                                                              CallbackData =
                                                                                  "finish_editing",
                                                                          },
                                                                  },
                                                          }
                                                  )
            );
        }

        public bool CanHandle(Update update) =>
            update.Message != null && update.Message.Text.Contains("/new");
    }
}