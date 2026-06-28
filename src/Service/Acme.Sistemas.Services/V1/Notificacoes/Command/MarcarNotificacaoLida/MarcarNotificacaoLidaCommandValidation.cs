using FluentValidation;

namespace Acme.Sistemas.Services.V1.Notificacoes.Command.MarcarNotificacaoLida;

public sealed class MarcarNotificacaoLidaCommandValidation
    : AbstractValidator<MarcarNotificacaoLidaCommand>
{
    public MarcarNotificacaoLidaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
