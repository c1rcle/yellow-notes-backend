namespace YellowNotes.Core.Utility
{
    public class EmailGenerator
    {
        public static EmailMessage RegistrationMessage(string emailAddress)
        {
            return new EmailMessage()
            {
                FromEmailAddress = "yellownotes@c1rcle.pl",
                ToEmailAddress = emailAddress,
                Subject = "Welcome to Yellow Notes!",
                Content = $"Hello {emailAddress}! Your account has been successfully created."
            };
        }

        public static EmailMessage PasswordChangeMessage(string emailAddress)
        {
            return new EmailMessage()
            {
                FromEmailAddress = "yellownotes@c1rcle.pl",
                ToEmailAddress = emailAddress,
                Subject = "Password change notification",
                Content = $"Hello {emailAddress}! Your password was recently changed."
            };
        }
    }
}
