namespace YellowNotes.Core
{
    public class EmailMessage
    {
        public string fromEmailAddress { get; set; }
        public string toEmailAddress { get; set; }
        public string subject { get; set; }
        public string content { get; set; }
    }
}
