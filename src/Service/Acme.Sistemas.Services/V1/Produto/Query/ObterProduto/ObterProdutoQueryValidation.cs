using FluentValidation;

namespace Acme.Sistemas.Services.V1.Produto.Query.ObterProduto;

public sealed class ObterProdutoQueryValidation : AbstractValidator<ObterProdutoQuery>
{
    public ObterProdutoQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
