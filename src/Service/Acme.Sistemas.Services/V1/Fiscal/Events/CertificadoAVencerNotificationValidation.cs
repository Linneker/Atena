using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fiscal.Events;

public sealed class CertificadoAVencerNotificationValidation : AbstractValidator<CertificadoAVencerNotification>
{
    public CertificadoAVencerNotificationValidation() { /* sem regras */ }
}
