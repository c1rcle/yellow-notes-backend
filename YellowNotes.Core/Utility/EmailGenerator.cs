using System.IO;

namespace YellowNotes.Core.Utility
{
  public class EmailGenerator
  {
    public static EmailMessage RegistrationMessage(string emailAddress) =>
      new EmailMessage()
      {
        FromEmailAddress = "yellownotes@c1rcle.pl",
        ToEmailAddress = emailAddress,
        Subject = "Welcome to Yellow Notes!",
        Content = File.ReadAllText("EmailTemplate.html")
      };

    public static EmailMessage PasswordChangeMessage(string emailAddress) =>
     new EmailMessage()
     {
       FromEmailAddress = "yellownotes@c1rcle.pl",
       ToEmailAddress = emailAddress,
       Subject = "Password change notification",
       Content = $"Hello {emailAddress}! Your password was recently changed."
     };
  }
}
