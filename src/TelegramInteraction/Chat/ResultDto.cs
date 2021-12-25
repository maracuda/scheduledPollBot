namespace TelegramInteraction.Chat
{
    public class ResultDto<TData, TError> : ResultDto<TError>
    {
        public TData Value { get; set; }
    }

    public class ResultDto<TError>
    {
        public bool Success { get; set; }
        public TError Error { get; set; }
    }
}