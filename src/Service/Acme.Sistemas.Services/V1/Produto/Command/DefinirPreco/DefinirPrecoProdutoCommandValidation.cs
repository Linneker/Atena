using FluentValidation;

namespace Acme.Sistemas.Services.V1.Produto.Command.DefinirPreco;

public sealed class DefinirPrecoProdutoCommandValidation : AbstractValidator<DefinirPrecoProdutoCommand>
{
    public DefinirPrecoProdutoCommandValidation()
    {
        RuleFor(x => x.ProdutoId).NotEmpty();
        RuleFor(x => x.TipoValorProdutoId).NotEmpty();
        RuleFor(x => x.Valor).GreaterThan(0);
    }
}
