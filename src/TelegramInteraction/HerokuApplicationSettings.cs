using System;

using BusinessLogic;

namespace TelegramInteraction
{
    public class HerokuApplicationSettings : IApplicationSettings
    {
        public string GetString(string name)
        {
            var variable = Environment.GetEnvironmentVariable(name);

            if(string.IsNullOrEmpty(variable))
            {
                throw new Exception($"Variable {name} not found in environment");
            }

            return variable;
        }
    }
}