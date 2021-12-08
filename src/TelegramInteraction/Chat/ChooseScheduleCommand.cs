using System;
using System.Threading.Tasks;

using BusinessLogic.CreatePolls;

using NCrontab;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramInteraction.Chat
{
    public class ChooseScheduleCommand : IChatCommand
    {
        public ChooseScheduleCommand(ICreatePollService createPollService,
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

            var scheduleText = update.Message.Text;

            if(TryParseSchedule(scheduleText, out var schedule))
            {
                pendingRequest.Schedule = schedule.ToString();

                await createPollService.SaveAsync(pendingRequest);
                
                await telegramBotClient.SendTextMessageAsync(chatId,
                                                             $"Next occurrence {schedule.GetNextOccurrence(DateTime.Now)} <b>UTC</b>",
                                                             parseMode: ParseMode.Html
                );
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
            && update.Message.ReplyToMessage.Text.StartsWith(ChatConstants.GuessScheduleText);

        private readonly ITelegramBotClient telegramBotClient;
        private readonly ICreatePollService createPollService;
        private readonly PollSender pollSender;
    }
}