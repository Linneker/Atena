using FluentValidation;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ListarSolicitacoes;

public sealed class ListarSolicitacoesQueryValidation : AbstractValidator<ListarSolicitacoesQuery>
{
    public ListarSolicitacoesQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
