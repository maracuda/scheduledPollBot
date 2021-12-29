using FluentAssertions;

namespace TelegramInteraction.Chat
{
    public class ChooseNameValidator : ValidatorBase<string>
    {
        protected override void ValidateInternal(string name)
        {
            name.Length.Should().BeLessOrEqualTo(300);
        }
    }
}