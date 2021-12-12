using System;
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
            var chatWorker = container.GetInstance<ChatWorker>();
            var logger = container.GetInstance<ITelegramLogger>();
            
            return DoWorkAsync(environment, chatWorker, logger);
        }

        private static async Task DoWorkAsync(IVostokHostingEnvironment environment, ChatWorker chatWorker, ITelegramLogger logger)
        {
            try
            {
                await chatWorker.DoWorkAsync(environment.ShutdownToken);
            }
            catch(Exception exception)
            {
                logger.Log(exception);
            }
        }
    }
}