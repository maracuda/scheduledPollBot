using System;
using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using Vostok.Logging.Abstractions;

namespace TelegramInteraction.Chat
{
    public class ChatWorker
    {
        public ChatWorker(ILog log,
                          ITelegramBotClient telegramBotClient,
                          ICommandsRouter commandsRouter, ITelegramLogger telegramLogger
        )
        {
            this.log = log;
            bot = telegramBotClient;
            this.commandsRouter = commandsRouter;
            this.telegramLogger = telegramLogger;
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            var me = await bot.GetMeAsync(cancellationToken);
            Console.Title = me.Username;

            bot.StartReceiving(UpdateHandler,
                               ErrorHandler,
                               new ReceiverOptions
                                   {
                                       AllowedUpdates = new[]
                                           {
                                               UpdateType.Message, UpdateType.CallbackQuery,
                                               
                                           },
                                   },
                               cancellationToken
            );

            log.Info($"Start listening for @{me.Username}");
        }

        private Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            log.Error(arg2);
            telegramLogger.Log(arg2);

            return Task.CompletedTask;
        }

        private async Task UpdateHandler(ITelegramBotClient arg1, Update arg2, CancellationToken arg3)
        {
            try
            {
                await commandsRouter.RouteAsync(arg2);
            }
            catch(Exception exception)
            {
                telegramLogger.Log(exception);
            }
        }

        private readonly ILog log;

        private readonly ITelegramBotClient bot;
        private readonly ICommandsRouter commandsRouter;
        private readonly ITelegramLogger telegramLogger;
    }
}