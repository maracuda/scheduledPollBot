using System;

using BusinessLogic;

namespace TelegramInteraction
{
    public static class ApplicationSettingsProvider
    {
        public static IApplicationSettings Get()
        {
            if(Environment.CurrentDirectory.Contains("heroku"))
            {
                return new HerokuApplicationSettings();
            }

            return new LocalApplicationSettings();
        }
    }
}