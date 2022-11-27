namespace BusinessLogic
{
    public static class ApplicationSettingsProvider
    {
        public static IApplicationSettings Get(string applicationIdentityEnvironment)
        {
            if(applicationIdentityEnvironment == EnvironmentType.Container)
            {
                return new HerokuApplicationSettings();
            }

            return new LocalApplicationSettings();
        }
    }
}