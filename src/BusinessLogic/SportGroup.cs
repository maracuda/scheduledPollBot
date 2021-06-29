namespace BusinessLogic
{
    public class SportGroup
    {
        /// <summary>
        /// Расписание занятий в Cron формате
        /// </summary>
        public string TrainingSchedule { get; set; }

        /// <summary>
        /// Расписание уведомлений о занятии в Cron формате
        /// </summary>
        public string NotificationSchedule { get; set; }

        /// <summary>
        /// Id чата в телеграмме
        /// </summary>
        public string TelegramChatId { get; set; }

        /// <summary>
        /// Имя группы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Короткое имя для опроса
        /// </summary>
        public string Title { get; set; }
    }
}