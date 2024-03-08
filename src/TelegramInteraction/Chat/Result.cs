namespace TelegramInteraction.Chat
{
    public class Result<TError>
    {
        public Result(bool isSuccess, TError error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public TError Error { get; }

        public bool Failure => !IsSuccess;

        public static Result<TError> Fail(TError error)
        {
            return new Result<TError>(false, error);
        }

        public static Result<TError> Ok()
        {
            return new Result<TError>(true, default);
        }

        public static Result<TError> Combine(params Result<TError>[] results)
        {
            foreach(var result in results)
            {
                if(result.Failure)
                {
                    return result;
                }
            }

            return Ok();
        }

        public static implicit operator Result<TError>(TError value)
        {
            return Fail(value);
        }
    }
}