using FluentValidation;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ObterSolicitacao;

public sealed class ObterSolicitacaoQueryValidation : AbstractValidator<ObterSolicitacaoQuery>
{
    public ObterSolicitacaoQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
