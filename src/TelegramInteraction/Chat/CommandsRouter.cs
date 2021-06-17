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
            var chatCommand = commands.SingleOrDefault(c => c.SupportedTemplates.Contains(commandText));

            if(chatCommand == null)
            {
                await telegramBotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Не знаю что ответить, попробуй что-то из знакомого:\r\n"
                          + $"{string.Join("\r\n", commands.SelectMany(c => c.SupportedTemplates))}"
                );
                return;
            }

            await chatCommand.ExecuteAsync(message);
        }

        private readonly ITelegramBotClient telegramBotClient;
        private readonly IChatCommand[] commands;
    }
}