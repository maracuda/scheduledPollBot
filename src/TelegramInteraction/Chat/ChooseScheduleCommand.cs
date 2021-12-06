using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using NCrontab;

using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class ChooseScheduleCommand : IChatCommand
    {
        private readonly ICreatePollService createPollService;
        private readonly PollSender pollSender;

        public ChooseScheduleCommand(ICreatePollService createPollService,
                                     PollSender pollSender
        )
        {
            this.createPollService = createPollService;
            this.pollSender = pollSender;
        }

        public async Task ExecuteAsync(Update update)
        {
            var chatId = update.Message.Chat.Id;

            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.Message.From.Id);

            var scheduleText = update.Message.Text;

            if(TryParseSchedule(scheduleText, out var schedule))
            {
                pendingRequest.Schedule = schedule.ToString();

                await createPollService.SaveAsync(pendingRequest);
                await pollSender.SendPollAsync(pendingRequest);
            }
        }

        private static bool TryParseSchedule(string scheduleText, out CrontabSchedule schedule)
        {
            try
            {
                schedule = CrontabSchedule.Parse(scheduleText);
                return true;
            }
            catch(CrontabException)
            {
                schedule = default;
                return false;
            }
        }

        public bool CanHandle(Update update) =>
            update.Message?.ReplyToMessage != null
            && update.Message.ReplyToMessage.Text == ChatConstants.GuessScheduleText;
    }
}