using System;
using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction.Chat
{
    public class CommandsRouter : ICommandsRouter
    {
        public CommandsRouter(ITelegramBotClient telegramBotClient,
                              IChatCommand[] commands
        )
        {
            this.telegramBotClient = telegramBotClient;
            this.commands = commands;
        }

        public async Task RouteAsync(Message message)
        {
            var commandText = message.Text.Split(' ').First();
            var chatCommands = commands.Where(c => c.CanHandle(message)).ToArray();

            if(chatCommands.Count() > 1)
            {
                throw new Exception(
                    $"Can't choose between commands "
                    + $"{string.Join(",", chatCommands.Select(c => c.GetType().Name))}"
                );
            }

            var command = chatCommands.SingleOrDefault();

            if(command != null)
            {
                await command.ExecuteAsync(message);
                return;
            }

            if(commandText.StartsWith("/"))
            {
                await telegramBotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Не знаю что ответить, попробуй что-то из знакомого:\r\n"
                          + $"{string.Join("\r\n", new[] { "/help", "/start", "/new" })}"
                );
            }
        }

        private readonly ITelegramBotClient telegramBotClient;
        private readonly IChatCommand[] commands;
    }
}