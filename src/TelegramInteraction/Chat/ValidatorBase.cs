using FluentAssertions.Execution;

namespace TelegramInteraction.Chat
{
    public abstract class ValidatorBase<T> : IValidator<T>
    {
        public Result<string> Validate(T smth)
        {
            try
            {
                ValidateInternal(smth);

                return Result<string>.Ok();
            }
            catch(AssertionFailedException exception)
            {
                return Result<string>.Fail(exception.Message);
            }
        }

        protected abstract void ValidateInternal(T smth);
    }
}