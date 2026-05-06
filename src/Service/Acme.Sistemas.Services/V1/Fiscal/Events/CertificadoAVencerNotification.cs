using Acme.Sistemas.Core.Mediators.Notification;

namespace Acme.Sistemas.Services.V1.Fiscal.Events;

public sealed record CertificadoAVencerNotification(
    Guid TenantId,
    string? Subject,
    DateTime ValidoAte,
    int DiasRestantes,
    DateTime DetectadoEm) : INotification;
