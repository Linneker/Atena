using FluentValidation;

namespace Acme.Sistemas.Services.V1.TipoProduto.Command.CriarTipoProduto;

public sealed class CriarTipoProdutoCommandValidation : AbstractValidator<CriarTipoProdutoCommand>
{
    public CriarTipoProdutoCommandValidation()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(2000);
    }
}
