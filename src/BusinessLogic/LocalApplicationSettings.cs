using System;
using System.IO;
using System.Linq;

namespace BusinessLogic
{
    public class LocalApplicationSettings : IApplicationSettings
    {
        public string GetString(string name)
        {
            var lines = File.ReadAllLines("Settings/settings.txt");

            var firstOrDefault = lines.FirstOrDefault(l => l.StartsWith(name));

            if(firstOrDefault == null)
            {
                throw new Exception($"Variable {name} not found in json file");
            }

            return firstOrDefault.Split("==", StringSplitOptions.RemoveEmptyEntries).Last().Trim();
        }
    }
}