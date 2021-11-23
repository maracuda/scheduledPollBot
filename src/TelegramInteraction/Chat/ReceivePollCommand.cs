using System.Linq;
using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class ReceivePollCommand : IChatCommand
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ICreatePollService createPollService;

        public ReceivePollCommand(ITelegramBotClient telegramBotClient, ICreatePollService createPollService)
        {
            this.telegramBotClient = telegramBotClient;
            this.createPollService = createPollService;
        }

        public async Task ExecuteAsync(Message message)
        {
            if(message.Poll == null)
            {
                await telegramBotClient.SendTextMessageAsync(message.Chat.Id, "There is no poll in message, send a poll please.");
                return;
            }

            var pendingRequest = await createPollService.FindPendingAsync(message.Chat.Id, message.From.Id);
            if(pendingRequest == null)
            {
                await telegramBotClient.SendTextMessageAsync(message.Chat.Id, "I can't find request to create. "
                                                                              + "Did you send me a /new?");
                return;
            }

            await createPollService.AddPollAsync(pendingRequest.Id, message.Poll);
            await telegramBotClient.SendPollAsync(message.Chat.Id,
                                                  message.Poll.Question,
                                                  message.Poll.Options.Select(o => o.Text),
                                                  isAnonymous: false
            );
        }

        public string[] SupportedTemplates => new[] { "/new" };
        public CommandType Type => CommandType.Poll;
    }
}