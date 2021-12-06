namespace TelegramInteraction.Chat
{
    public static class ChatConstants
    {
        public const string AnonymousCallback = "change_anonymous";
        public const string NameCallback = "change_name";
        public const string GuessNameText = "Send me a name";
        public static string OptionsCallback => "change_options";
        public static string GuessOptionsText => "Send me options, one each line";
        public static string ScheduleCallback => "change_schedule";
        public static string GuessScheduleText => "Send me a schedule in cron format";
    }
}