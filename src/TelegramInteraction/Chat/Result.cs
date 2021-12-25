namespace TelegramInteraction.Chat
{
    public class Result<TData, TError> : Result<TError>
    {
        public Result(TData value, bool isSuccess, TError error)
            : base(isSuccess, error) => Value = value;

        public TData Value { get; }

        public static Result<TData, TError> Ok(TData value)
        {
            return new Result<TData, TError>(value, true, default);
        }

        public static Result<TData, TError> Fail(TData value, TError error)
        {
            return new Result<TData, TError>(value, false, error);
        }

        public new static Result<TData, TError> Fail(TError error)
        {
            return new Result<TData, TError>(default, false, error);
        }

        public static implicit operator Result<TData, TError>(TData value)
        {
            return Ok(value);
        }

        public static implicit operator Result<TData, TError>(TError value)
        {
            return Fail(value);
        }
    }

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