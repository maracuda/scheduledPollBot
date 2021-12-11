using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TelegramInteraction.Chat
{
    public class ScheduledPollService : IScheduledPollService
    {
        private readonly List<ScheduledPoll> scheduledPolls;

        public ScheduledPollService()
        {
            scheduledPolls = new List<ScheduledPoll>();
        }

        public async Task CreateAsync(ScheduledPoll scheduledPoll)
        {
            scheduledPolls.Add(scheduledPoll);
            await File.WriteAllTextAsync("db.json", JsonConvert.SerializeObject(scheduledPolls));
        }

        public async Task<ScheduledPoll[]> ReadAllAsync()
        {
            var text = await File.ReadAllTextAsync("db.json");

            return JsonConvert.DeserializeObject<ScheduledPoll[]>(text);
        }
    }
}