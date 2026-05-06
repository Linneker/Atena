using FluentValidation;

namespace Acme.Sistemas.Services.V1.TipoValorProduto.Command.CriarTipoValorProduto;

public sealed class CriarTipoValorProdutoCommandValidation : AbstractValidator<CriarTipoValorProdutoCommand>
{
    public CriarTipoValorProdutoCommandValidation()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(2000);
    }
}
