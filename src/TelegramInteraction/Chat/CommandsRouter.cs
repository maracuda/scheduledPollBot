using System;
using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;

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

        public async Task RouteAsync(UpdateEventArgs updateEventArgs)
        {
            var update = updateEventArgs.Update;

            var chatCommands = commands.Where(c => c.CanHandle(update)).ToArray();

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
                await command.ExecuteAsync(update);
                return;
            }

            /*
            var commandText = message.Text.Split(' ').First();
            if(commandText.StartsWith("/"))
            {
                await telegramBotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Не знаю что ответить, попробуй что-то из знакомого:\r\n"
                          + $"{string.Join("\r\n", new[] { "/help", "/start", "/new" })}"
                );
            }*/
            ;
        }

        // ReSharper disable once NotAccessedField.Local
        private readonly ITelegramBotClient telegramBotClient;
        private readonly IChatCommand[] commands;
    }
}