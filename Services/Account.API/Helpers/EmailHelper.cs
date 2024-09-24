using MimeKit;

namespace Account.API.Helpers;

/// <summary>
/// Email helper
/// </summary>
public abstract class EmailHelper
{
    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    public string? HtmlFilePath { get; set; }

    /// <summary>
    /// Email helper
    /// </summary>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="content"></param>
    public EmailHelper(IEnumerable<string> to, string subject, string content)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(x => new MailboxAddress("email", x)));
        Subject = subject;
        Content = content;
    }
}