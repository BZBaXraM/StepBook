using MimeKit;

namespace StepBook.API.Repositories.Interfaces;

/// <summary>
/// Email service
/// </summary>
public interface IEmailRepository
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