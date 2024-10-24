using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using StepBook.API.Data.Configs;
using StepBook.BLL.Helpers;

namespace StepBook.BLL.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfig _config;

    public EmailService(IOptions<EmailConfig> options)
    {
        _config = options.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Step", _config.From));
        emailMessage.To.Add(new MailboxAddress("User", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using var client = new SmtpClient();
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

        await client.ConnectAsync(_config.SmtpServer, _config.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_config.UserName, _config.Password);
        await client.SendAsync(emailMessage);

        await client.DisconnectAsync(true);
    }

    public async Task SendAsync(MimeMessage message)
    {
        using var client = new SmtpClient();
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

        await client.ConnectAsync(_config.SmtpServer, _config.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_config.UserName, _config.Password);
        await client.SendAsync(message);

        await client.DisconnectAsync(true);
    }

    private MimeMessage CreateEmailMessage(EmailHelper helper, string confirmLink)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Step", _config.From));
        emailMessage.To.AddRange(helper.To);
        emailMessage.Subject = helper.Subject;

        var htmlContent = File.ReadAllText(helper.HtmlFilePath!);
        htmlContent = htmlContent.Replace("{{Link}}", confirmLink);

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlContent
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();

        return emailMessage;
    }
}