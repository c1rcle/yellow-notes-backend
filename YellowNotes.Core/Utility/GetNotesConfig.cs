namespace YellowNotes.Core.Utility
{
    public class GetNotesConfig
    {
        public int TakeCount { get; set; } = 20;

        public int SkipCount { get; set; } = 0;

        public int[] CategoryIds { get; set; }
    }
}
