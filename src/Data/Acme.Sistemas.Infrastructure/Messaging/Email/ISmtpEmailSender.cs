using Acme.Sistemas.Domain.Interfaces.Messaging;

namespace Acme.Sistemas.Infrastructure.Messaging.Email;

public interface ISmtpEmailSender
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
