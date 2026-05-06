using FluentValidation;

namespace Acme.Sistemas.Services.V1.Produto.Command.ExcluirProduto;

public sealed class ExcluirProdutoCommandValidation : AbstractValidator<ExcluirProdutoCommand>
{
    public ExcluirProdutoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
