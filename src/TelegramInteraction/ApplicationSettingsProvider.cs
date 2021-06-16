using BusinessLogic;

namespace TelegramInteraction
{
    public static class ApplicationSettingsProvider
    {
        public static IApplicationSettings Get(string applicationIdentityEnvironment)
        {
            if(applicationIdentityEnvironment == EnvironmentType.Production)
            {
                return new HerokuApplicationSettings();
            }

            return new LocalApplicationSettings();
        }
    }
}