using FluentValidation;

namespace Acme.Sistemas.Services.V1.Faturamento.Events;

public sealed class NFeEmissaoSolicitadaNotificationValidation : AbstractValidator<NFeEmissaoSolicitadaNotification>
{
    public NFeEmissaoSolicitadaNotificationValidation() { /* sem regras */ }
}
