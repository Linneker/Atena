using FluentValidation;

namespace Acme.Sistemas.Services.V1.Produto.Command.CriarProduto;

public sealed class CriarProdutoCommandValidation : AbstractValidator<CriarProdutoCommand>
{
    public CriarProdutoCommandValidation()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(2000);
        RuleFor(x => x.CodigoBarras).MaximumLength(64);
        RuleFor(x => x.UnidadeMedida).NotEmpty().MaximumLength(10);
        RuleFor(x => x.CustoMedio).GreaterThanOrEqualTo(0).When(x => x.CustoMedio.HasValue);
        RuleFor(x => x.EstoqueMinimo).GreaterThanOrEqualTo(0).When(x => x.EstoqueMinimo.HasValue);
    }
}
