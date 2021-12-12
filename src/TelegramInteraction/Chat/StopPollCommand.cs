using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramInteraction.Chat
{
    public class StopPollCommand : IChatCommand
    {
        public StopPollCommand(ITelegramBotClient telegramBotClient, IScheduledPollService scheduledPollService
        )
        {
            this.telegramBotClient = telegramBotClient;
            this.scheduledPollService = scheduledPollService;
        }

        public async Task ExecuteAsync(Update update)
        {
            if(update.Message.Chat.Type == ChatType.Private)
            {
                await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id,
                                                             "This command available only in group"
                );
                return;
            }
            
            var pollsInGroup = await scheduledPollService.GetAll(update.Message.Chat.Id);
            var activePoll = pollsInGroup.FirstOrDefault(p => !p.IsDisabled);
            
            if(activePoll != null)
            {
                activePoll.IsDisabled = true;
                await scheduledPollService.SaveAsync(activePoll);

                await telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"Poll *{activePoll.Name}* was stopped"
                                                                 + $"\r\n To start polling again create a new one by /new");
            }
        }

        public bool CanHandle(Update update) =>
            update.Message != null
            && update.Message.Text.Contains("/stop");

        private readonly ITelegramBotClient telegramBotClient;
        private readonly IScheduledPollService scheduledPollService;
    }
}