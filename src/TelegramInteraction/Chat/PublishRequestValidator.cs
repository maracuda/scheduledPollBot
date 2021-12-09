using BusinessLogic.CreatePolls;

using FluentAssertions;
using FluentAssertions.Execution;

namespace TelegramInteraction.Chat
{
    public class PublishRequestValidator : IPublishRequestValidator
    {
        public Result<string> Validate(CreatePollRequest poll)
        {
            try
            {
                poll.Options.Length.Should().BeGreaterOrEqualTo(2);
                poll.Options.Should()
                              .NotBeEquivalentTo(new[]
                                      {
                                          DefaultPollConstants.FirstOption,
                                          DefaultPollConstants.SecondOption,
                                      }
                              );
                
                poll.PollName.Should().NotBe(DefaultPollConstants.Name);
                poll.Schedule.Should().NotBeNull();
                
                return Result<string>.Ok();
            }
            catch(AssertionFailedException exception)
            {
                return Result<string>.Fail(exception.Message);
            }
        }
    }
}