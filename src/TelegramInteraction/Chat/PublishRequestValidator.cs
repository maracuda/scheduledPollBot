using System.Collections;
using System.Text;

using BusinessLogic.CreatePolls;

using FluentAssertions;
using FluentAssertions.Execution;

namespace TelegramInteraction.Chat
{
    public class PublishRequestValidator : IPublishRequestValidator
    {
        public Result<string> Validate(CreatePollRequest pendingRequest)
        {
            try
            {
                pendingRequest.Options.Length.Should().BeGreaterOrEqualTo(2);
                pendingRequest.Options.Should()
                              .NotBeEquivalentTo(new[]
                                      {
                                          DefaultPollConstants.FirstOption,
                                          DefaultPollConstants.SecondOption,
                                      }
                              );
                return Result<string>.Ok();
            }
            catch(AssertionFailedException exception)
            {
                var stringBuilder = new StringBuilder();
                foreach(DictionaryEntry o in exception.Data)
                {
                    stringBuilder.Append($"{o.Key}: {o.Value}");
                }

                return Result<string>.Fail(stringBuilder.ToString());
            }
        }
    }
}