using FluentValidation;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.RejeitarSolicitacao;

public sealed class RejeitarSolicitacaoCommandValidation : AbstractValidator<RejeitarSolicitacaoCommand>
{
    public RejeitarSolicitacaoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Motivo).NotEmpty().MaximumLength(2000);
    }
}
