using System.Threading.Tasks;

using SimpleInjector;

using Vostok.Hosting.Abstractions;

namespace TelegramInteraction.Chat
{
    public class ChatApplication : IVostokApplication
    {

        public Task InitializeAsync(IVostokHostingEnvironment environment)
        {
            return Task.CompletedTask;
        }

        public Task RunAsync(IVostokHostingEnvironment environment)
        {
            var container = environment.HostExtensions.Get<Container>();
            return container.GetInstance<ChatWorker>().DoWorkAsync(environment.ShutdownToken);
        }
    }
}