using FluentValidation;

namespace Acme.Sistemas.Services.V1.Produto.Command.AlterarProduto;

public sealed class AlterarProdutoCommandValidation : AbstractValidator<AlterarProdutoCommand>
{
    public AlterarProdutoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.UnidadeMedida).NotEmpty().MaximumLength(10);
    }
}
