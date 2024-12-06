using MimeKit;

namespace StepBook.API.Services;

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
    /// <summary>
    /// Send confirmation email
    /// </summary>
    /// <param name="email"></param>
    /// <param name="confirmLink"></param>
    /// <returns></returns>
    Task SendConfirmationEmailAsync(string email, string confirmLink);
}