namespace YellowNotes.Core
{
    public class EmailMessage
    {
        public string FromEmailAddress { get; set; }

        public string ToEmailAddress { get; set; }

        public string Subject { get; set; }
        
        public string Content { get; set; }
    }
}
