namespace BusinessLogic.CreatePolls
{
    public interface IPollContextFactory
    {
        PollsContext Create();
    }
}