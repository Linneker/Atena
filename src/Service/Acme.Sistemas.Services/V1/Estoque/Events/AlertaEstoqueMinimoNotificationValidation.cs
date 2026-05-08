using FluentValidation;

namespace Acme.Sistemas.Services.V1.Estoque.Events;

public sealed class AlertaEstoqueMinimoNotificationValidation : AbstractValidator<AlertaEstoqueMinimoNotification>
{
    public AlertaEstoqueMinimoNotificationValidation() { /* sem regras */ }
}
