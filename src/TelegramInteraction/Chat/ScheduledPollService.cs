using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TelegramInteraction.Chat
{
    public class ScheduledPollService : IScheduledPollService
    {
        public async Task CreateAsync(ScheduledPoll scheduledPoll)
        {
            var allPolls = await ReadAllAsync();

            var polls = allPolls.ToList();
            polls.Add(scheduledPoll);
            
            await File.WriteAllTextAsync("db.json", JsonConvert.SerializeObject(polls.ToArray()));
        }

        public async Task<ScheduledPoll[]> ReadAllAsync()
        {
            var text = await File.ReadAllTextAsync("db.json");

            return JsonConvert.DeserializeObject<ScheduledPoll[]>(text) ?? Array.Empty<ScheduledPoll>();
        }

        public async Task<ScheduledPoll[]> GetAll(long chatId)
        {
            var all = await ReadAllAsync();
            return all.Where(p => p.ChatId == chatId).ToArray();
        }

        public async Task SaveAsync(ScheduledPoll poll)
        {
            var allPolls = (await ReadAllAsync()).ToList();
            
            var oldPoll = allPolls.FirstOrDefault(p => p.Id == poll.Id);
            if(oldPoll == null)
            {
                return;
            }

            allPolls.Remove(oldPoll);
            allPolls.Add(poll);
            
            await File.WriteAllTextAsync("db.json", JsonConvert.SerializeObject(allPolls.ToArray()));
        }
    }
}