namespace TelegramInteraction.Chat
{
    public interface IValidator<T>
    {
        Result<string> Validate(T smth);
    }
}