using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fornecedor.Query.ObterFornecedor;

public sealed class ObterFornecedorQueryValidation : AbstractValidator<ObterFornecedorQuery>
{
    public ObterFornecedorQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
