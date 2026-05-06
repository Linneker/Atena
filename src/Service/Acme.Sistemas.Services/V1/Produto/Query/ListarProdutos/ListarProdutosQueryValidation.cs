using FluentValidation;

namespace Acme.Sistemas.Services.V1.Produto.Query.ListarProdutos;

public sealed class ListarProdutosQueryValidation : AbstractValidator<ListarProdutosQuery>
{
    public ListarProdutosQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
        RuleFor(x => x.Termo).MaximumLength(100);
    }
}
