using FluentAssertions;
using FluentAssertions.Execution;

namespace TelegramInteraction.Chat
{
    public class ChooseOptionsValidator : IValidator<string[]>
    {
        public Result<string> Validate(string[] smth)
        {
            try
            {
                smth.Length.Should().BeGreaterOrEqualTo(2);
                smth.Should().OnlyContain(o => !string.IsNullOrEmpty(o));
                smth.Should().OnlyContain(o => o.Length <= 100);

                return Result<string>.Ok();
            }
            catch(AssertionFailedException exception)
            {
                return Result<string>.Fail(exception.Message);
            }
        }
    }
}