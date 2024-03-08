using System;

namespace BusinessLogic
{
    public class HoustonApplicationSettings : IApplicationSettings
    {
        public HoustonApplicationSettings(MySettings settings)
        {
            this.settings = settings;
        }

        public string GetString(string name)
        {
            if(name == "BotToken")
            {
                return settings.BotToken;
            }

            if(name == "AdminChatId")
            {
                return settings.AdminChatId;
            }

            throw new NotImplementedException("нет такой настройки");
        }

        private readonly MySettings settings;
    }
}