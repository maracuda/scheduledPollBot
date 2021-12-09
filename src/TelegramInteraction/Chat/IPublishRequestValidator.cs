using BusinessLogic.CreatePolls;

namespace TelegramInteraction.Chat
{
    public interface IPublishRequestValidator
    {
        Result<string> Validate(CreatePollRequest pendingRequest);
    }
}