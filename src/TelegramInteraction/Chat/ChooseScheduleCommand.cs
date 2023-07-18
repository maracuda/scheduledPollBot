using System;
using System.Collections.Generic;
using System.Linq;
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

            var pendingRequest = await createPollService.FindPendingAsync(chatId, update.Message.From.Id); if(pendingRequest == null)
            {
                await telegramBotClient.SendTextMessageAsync(chatId, $"Sorry, there is no poll from you, to create one use /new");
                return;
            }

            var scheduleText = update.Message.Text;

            if(TryParseSchedule(scheduleText, out var schedule))
            {
                pendingRequest.Schedule = schedule.ToString();
                await createPollService.SaveAsync(pendingRequest);

                var next3StartTimes = string.Join("\r\n", GetSchedules(DateTime.UtcNow, schedule)
                                                .Take(3)
                                                .Select((dt, index) => $"{index + 1} poll will be at: {dt.ToString().Replace(".", "\\.")}"));

                await telegramBotClient.SendTextMessageAsync(chatId,
                                                             $"Ok, schedule set up\r\nCheck schedule for next 3 times, TimeZone is *UTC*:\r\n{next3StartTimes}\r\nAnd it will never stop, until you call /stop",
                                                             parseMode: ParseMode.MarkdownV2
                );
            }
            else
            {
                await telegramBotClient.SendTextMessageAsync(chatId, $"Schedule is invalid, please correct it");
            }
            
            await pollSender.SendPollAsync(pendingRequest);
        }
        
        private IEnumerable<DateTime> GetSchedules(DateTime start, CrontabSchedule schedule)
        {
            var nextOccurrence = schedule.GetNextOccurrence(start);
            while(true)
            {
                yield return nextOccurrence;
                nextOccurrence = schedule.GetNextOccurrence(nextOccurrence);
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
            update.Message?.ReplyToMessage?.Text != null 
            && update.Message.ReplyToMessage.Text.StartsWith(ChatConstants.GuessScheduleText);

        private readonly ITelegramBotClient telegramBotClient;
        private readonly ICreatePollService createPollService;
        private readonly PollSender pollSender;
    }
}