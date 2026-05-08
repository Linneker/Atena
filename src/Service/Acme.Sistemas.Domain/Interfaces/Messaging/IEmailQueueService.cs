namespace Acme.Sistemas.Domain.Interfaces.Messaging;

public interface IEmailQueueService
{
    Task EnqueueAsync(EmailMessage message, CancellationToken cancellationToken = default);
}

public sealed record EmailMessage(
    string To,
    string Subject,
    string Body,
    bool IsHtml = true,
    IReadOnlyList<EmailAttachment>? Attachments = null);

public sealed record EmailAttachment(string FileName, byte[] Content, string ContentType);
