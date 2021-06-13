using System;
using System.Linq;
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
        private readonly ILog log;

        public ChatWorker(ILog log,
                          ITelegramBotClient telegramBotClient
        )
        {
            this.log = log;
            bot = telegramBotClient;
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            var me = await bot.GetMeAsync(cancellationToken);
            Console.Title = me.Username;

            bot.OnReceiveError += (_, args) => log.Error(args.ApiRequestException);
            bot.OnMessage += BotOnMessageReceived;
            
            bot.StartReceiving(null, cancellationToken);
            log.Info($"Start listening for @{me.Username}");
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if(message == null || message.Type != MessageType.Text)
            {
                return;
            }

            switch(message.Text.Split(' ').First())
            {
            case "/ping":
                await bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "pong"
                );
                break;
            }
        }

        private readonly ITelegramBotClient bot;
    }
}