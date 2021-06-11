using System;
using System.Threading.Tasks;

using Vostok.Applications.Scheduled;
using Vostok.Hosting.Abstractions;
using Vostok.Logging.Abstractions;

namespace TelegramInteraction
{
    public class ScheduledApplication : VostokScheduledAsyncApplication
    {
        protected override Task SetupAsync(IScheduledActionsBuilder builder, IVostokHostingEnvironment environment)
        {
            // Здесь следует регистрировать задачи через переданный builder.
            // Для регистрации нужно передать имя, планировщик и делегат с полезной нагрузкой.
            // Тут же можно провести и необходимый "прогрев" приложения.
 
            builder.Schedule("action1", Scheduler.Periodical(TimeSpan.FromSeconds(10)),
                             () =>
                                 {
                                     environment.Log.Info("I'm running!");
                                 });
         
            return Task.CompletedTask;
        }
 }
}