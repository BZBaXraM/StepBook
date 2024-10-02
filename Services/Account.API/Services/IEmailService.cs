using MimeKit;

namespace Account.API.Services;

/// <summary>
/// Email service
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send email
    /// </summary>
    /// <param name="email"></param>
    /// <param name="subject"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendEmailAsync(string email, string subject, string message);
    /// <summary>
    /// Send email
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendAsync(MimeMessage message);
}