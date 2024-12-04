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
}