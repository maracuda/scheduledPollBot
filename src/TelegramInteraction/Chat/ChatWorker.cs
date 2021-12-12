using System;
using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
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

            bot.OnReceiveError += (_, args) =>
                {
                    log.Error(args.ApiRequestException);
                    telegramLogger.Log(args.ApiRequestException);
                };
            bot.OnUpdate += BotOnMessageReceived;

            bot.StartReceiving(new [] {UpdateType.Message, UpdateType.CallbackQuery}, cancellationToken);
            log.Info($"Start listening for @{me.Username}");
        }

        private async void BotOnMessageReceived(object? sender, UpdateEventArgs updateEventArgs)
        {
            try
            {
                await commandsRouter.RouteAsync(updateEventArgs);
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