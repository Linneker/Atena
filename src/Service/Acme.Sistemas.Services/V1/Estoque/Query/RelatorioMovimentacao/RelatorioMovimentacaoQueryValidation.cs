using FluentValidation;

namespace Acme.Sistemas.Services.V1.Estoque.Query.RelatorioMovimentacao;

public sealed class RelatorioMovimentacaoQueryValidation : AbstractValidator<RelatorioMovimentacaoQuery>
{
    public RelatorioMovimentacaoQueryValidation()
    {
        RuleFor(x => x.ProdutoId).NotEmpty();
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 1000);
    }
}
