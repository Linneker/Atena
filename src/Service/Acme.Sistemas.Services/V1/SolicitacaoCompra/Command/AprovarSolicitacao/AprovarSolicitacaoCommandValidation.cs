using FluentValidation;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.AprovarSolicitacao;

public sealed class AprovarSolicitacaoCommandValidation : AbstractValidator<AprovarSolicitacaoCommand>
{
    public AprovarSolicitacaoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
