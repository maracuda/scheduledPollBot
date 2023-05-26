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
                                                  allowsMultipleAnswers: createPollRequest.AllowsMultipleAnswers,
                                                  replyMarkup: new InlineKeyboardMarkup(
                                                      new[]
                                                          {
                                                              new[]
                                                                  {
                                                                      new InlineKeyboardButton("Имя опроса")
                                                                          {
                                                                              CallbackData = ChatConstants.NameCallback,
                                                                          },
                                                                      new InlineKeyboardButton(createPollRequest.IsAnonymous ? "Сделать неанонимным" : "Сделать анонимным")
                                                                          {
                                                                              CallbackData = ChatConstants.AnonymousCallback,
                                                                          },
                                                                  },
                                                              new []
                                                                  {
                                                                      new InlineKeyboardButton(
                                                                          createPollRequest.AllowsMultipleAnswers
                                                                              ? "Разрешить единственный выбор"
                                                                              : "Разрешить множественный выбор"
                                                                      )
                                                                          {
                                                                              CallbackData = ChatConstants.MultipleCallback,
                                                                          },
                                                                  },
                                                              new[]
                                                                  {
                                                                      new InlineKeyboardButton("Ответы")
                                                                          {
                                                                              CallbackData = ChatConstants.OptionsCallback,
                                                                          },
                                                                      new InlineKeyboardButton("Расписание")
                                                                          {
                                                                              CallbackData = ChatConstants.ScheduleCallback,
                                                                          },
                                                                  },
                                                              new[]
                                                                  {
                                                                      new InlineKeyboardButton("Жми сюда когда всё готово!")
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