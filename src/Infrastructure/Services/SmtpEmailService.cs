using Application.Interfaces.Services;
using Application.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IOptions<EmailOptions> emailOptions, ILogger<SmtpEmailService> logger)
    {
        _emailOptions = emailOptions.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody, CancellationToken ct)
    {
        using var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailOptions.SenderEmail, _emailOptions.SenderName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        using var smtpClient = new SmtpClient(_emailOptions.SmtpHost, _emailOptions.SmtpPort)
        {
            Credentials = new NetworkCredential(_emailOptions.Username, _emailOptions.Password),
            EnableSsl = _emailOptions.EnableSsl
        };

        _logger.LogInformation("Sending email to {Email}", toEmail);
        await smtpClient.SendMailAsync(mailMessage, ct);
    }
}
