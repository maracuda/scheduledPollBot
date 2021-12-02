using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramInteraction.Chat
{
    public class PollSender
    {
        private ITelegramBotClient telegramBotClient;

        public PollSender(ITelegramBotClient telegramBotClient)
        {
            this.telegramBotClient = telegramBotClient;
        }

        public async Task SendPollAsync(CreatePollRequest createPollRequest)
        {
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
                                                                              CallbackData = ChatConstants.NameCallback,
                                                                              SwitchInlineQueryCurrentChat = "test", 
                                                                              
                                                                          },
                                                                      new InlineKeyboardButton
                                                                          {
                                                                              Text = "Anonymous",
                                                                              CallbackData = ChatConstants.AnonymousCallback,
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
    }
}