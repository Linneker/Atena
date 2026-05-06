namespace Acme.Sistemas.Infrastructure.Messaging.Email;

public interface ISmtpEmailSender
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
