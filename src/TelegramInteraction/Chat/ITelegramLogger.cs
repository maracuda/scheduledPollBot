using System;

namespace TelegramInteraction.Chat
{
    public interface ITelegramLogger
    {
        void Log(Exception exception);
    }
}