using System.Threading.Tasks;

namespace BusinessLogic
{
    public class HardCodedSportGroupRepository : ISportGroupRepository
    {
        public Task<SportGroup[]> ReadAllAsync()
        {
            return Task.FromResult(new[]
                    {
                        // NOTE: в Heroku время по utc, поэтому во всех расписаниях время по utc
                        new SportGroup
                            {
                                Name = "A.Petrazhickiy",
                                TrainingSchedule = "0 5 * * 2,5",
                                NotificationSchedule = "0 5 * * 1,4",
                                TelegramChatId = "-1001318882712",
                                Title = "Йога",
                                PollOptions = new[] {"Пойду в зал", "Пойду онлайн", "Не пойду", "Возможно",},
                            },
                        new SportGroup
                            {
                                Name = "E.Petrazhickaya",
                                TrainingSchedule = "0 5 * * 2,4",
                                NotificationSchedule = "0 5 * * 1,3",
                                TelegramChatId = "-1001286172835",
                                Title = "Йога",
                                PollOptions = new[] {"Пойду в зал", "Пойду онлайн", "Не пойду", "Возможно",},
                            },
                        new SportGroup
                            {
                                Name = "Anna Vetlugina",
                                Title = "Фитнес",
                                TrainingSchedule = "0 4 * * 1,3,5",
                                NotificationSchedule = "0 5 * * 5,2,4",
                                PollOptions = new[] {"Зал", "Онлайн", "Не приду"},
                                TelegramChatId = "-1001332112170",
                            },
                        new SportGroup
                            {
                                Name = "Anna Vetlugina evening",
                                Title = "Вечерний фитнес",
                                TrainingSchedule = "0 14 * * 2,4",
                                NotificationSchedule = "0 5 * * 2,4",
                                PollOptions = new[] {"Зал", "Онлайн", "Не приду"},
                                TelegramChatId = "-1001290388650",
                            },
                        new SportGroup
                            {
                                Name = "E.Petrazhickaya on NV",
                                Title = "Йога",
                                TrainingSchedule = "15 7 * * 1,3,5",
                                NotificationSchedule = "0 5 * * 0,2,4",
                                PollOptions = new[] {"Пойду в зал", "Пойду онлайн", "Не пойду", "Возможно",},
                                TelegramChatId = "-389442739",
                            },
                    }
            );
        }
    }
}