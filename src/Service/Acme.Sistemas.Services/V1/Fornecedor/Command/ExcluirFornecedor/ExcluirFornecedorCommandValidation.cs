using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fornecedor.Command.ExcluirFornecedor;

public sealed class ExcluirFornecedorCommandValidation : AbstractValidator<ExcluirFornecedorCommand>
{
    public ExcluirFornecedorCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
