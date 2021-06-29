using System;

using Vostok.Applications.Scheduled;

namespace TelegramInteraction
{
    public class CrontabWithOffsetScheduler : IScheduler
    {
        public CrontabWithOffsetScheduler(string schedule, TimeSpan offset)
        {
            this.schedule = schedule;
            this.offset = offset;
        }

        public DateTimeOffset? ScheduleNext(DateTimeOffset from)
        {
            var nextByCrontab = Scheduler.Crontab(schedule).ScheduleNext(from);
            if(!nextByCrontab.HasValue)
            {
                throw new Exception($"Can't define next execution from schedule: {schedule}");
            }

            return nextByCrontab.Value.Add(offset);
        }

        private string schedule;
        private TimeSpan offset;
    }
}