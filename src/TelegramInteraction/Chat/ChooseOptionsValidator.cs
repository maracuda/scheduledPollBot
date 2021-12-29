using FluentAssertions;

namespace TelegramInteraction.Chat
{
    public class ChooseOptionsValidator : ValidatorBase<string[]>
    {
        protected override void ValidateInternal(string[] options)
        {
            options.Length.Should().BeGreaterOrEqualTo(2);
            options.Should().OnlyContain(o => !string.IsNullOrEmpty(o));
            options.Should().OnlyContain(o => o.Length <= 100);
        }
    }
}