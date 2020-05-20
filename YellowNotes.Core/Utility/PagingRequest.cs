namespace YellowNotes.Core.Utility
{
    public abstract class PagingRequest
    {
        public int TakeCount { get; set; } = 20;

        public int SkipCount { get; set; } = 0;
    }
}
