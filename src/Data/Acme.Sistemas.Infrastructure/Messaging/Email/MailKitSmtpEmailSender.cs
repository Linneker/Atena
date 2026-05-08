using Acme.Sistemas.Domain.Interfaces.Messaging;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Acme.Sistemas.Infrastructure.Messaging.Email;

public sealed class MailKitSmtpEmailSender : ISmtpEmailSender
{
    private readonly SmtpOptions _options;
    private readonly ILogger<MailKitSmtpEmailSender> _logger;

    public MailKitSmtpEmailSender(IOptions<SmtpOptions> options, ILogger<MailKitSmtpEmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        var mime = new MimeMessage();
        mime.From.Add(new MailboxAddress(_options.FromDisplayName, _options.FromAddress));
        mime.To.Add(MailboxAddress.Parse(message.To));
        mime.Subject = message.Subject;

        var builder = new BodyBuilder();
        if (message.IsHtml)
            builder.HtmlBody = message.Body;
        else
            builder.TextBody = message.Body;

        if (message.Attachments is not null)
        {
            foreach (var att in message.Attachments)
            {
                builder.Attachments.Add(att.FileName, att.Content, ContentType.Parse(att.ContentType));
            }
        }

        mime.Body = builder.ToMessageBody();

        using var client = new SmtpClient
        {
            Timeout = (int)TimeSpan.FromSeconds(_options.TimeoutSeconds).TotalMilliseconds
        };

        var secureSocketOptions = _options.UseStartTls
            ? SecureSocketOptions.StartTlsWhenAvailable
            : SecureSocketOptions.Auto;

        await client.ConnectAsync(_options.Host, _options.Port, secureSocketOptions, cancellationToken);

        if (!string.IsNullOrEmpty(_options.Username))
        {
            await client.AuthenticateAsync(_options.Username, _options.Password ?? string.Empty, cancellationToken);
        }

        await client.SendAsync(mime, cancellationToken);
        await client.DisconnectAsync(quit: true, cancellationToken);

        _logger.LogInformation("E-mail enviado para {To} ({Subject}).", message.To, message.Subject);
    }
}
