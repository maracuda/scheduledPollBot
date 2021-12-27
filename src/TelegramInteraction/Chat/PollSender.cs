using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramInteraction.Chat
{
    public class PollSender
    {
        private readonly ITelegramBotClient telegramBotClient;

        public PollSender(ITelegramBotClient telegramBotClient)
        {
            this.telegramBotClient = telegramBotClient;
        }

        public async Task SendPollAsync(CreatePollRequest createPollRequest)
        {
            await telegramBotClient.SendPollAsync(createPollRequest.ChatId,
                                                  createPollRequest.PollName,
                                                  createPollRequest.Options,
                                                  createPollRequest.IsAnonymous,
                                                  replyMarkup: new InlineKeyboardMarkup(
                                                      new[]
                                                          {
                                                              new[]
                                                                  {
                                                                      new InlineKeyboardButton("Name")
                                                                          {
                                                                              CallbackData = ChatConstants.NameCallback,
                                                                          },
                                                                      new InlineKeyboardButton(createPollRequest.IsAnonymous ? "Non-anonymous" : "Anonymous")
                                                                          {
                                                                              CallbackData = ChatConstants.AnonymousCallback,
                                                                          },
                                                                  },
                                                              new[]
                                                                  {
                                                                      new InlineKeyboardButton("Options")
                                                                          {
                                                                              CallbackData = ChatConstants.OptionsCallback,
                                                                          },
                                                                      new InlineKeyboardButton("Schedule")
                                                                          {
                                                                              CallbackData = ChatConstants.ScheduleCallback,
                                                                          },
                                                                  },
                                                              new[]
                                                                  {
                                                                      new InlineKeyboardButton("Publish")
                                                                          {
                                                                              CallbackData = ChatConstants.PublishCallback,
                                                                          },
                                                                  },
                                                          }
                                                  )
            );
        }

    }
}