using FluentValidation;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Events;

public sealed class NotificarAprovacaoPendenteNotificationValidation : AbstractValidator<NotificarAprovacaoPendenteNotification>
{
    public NotificarAprovacaoPendenteNotificationValidation() { /* sem regras */ }
}
