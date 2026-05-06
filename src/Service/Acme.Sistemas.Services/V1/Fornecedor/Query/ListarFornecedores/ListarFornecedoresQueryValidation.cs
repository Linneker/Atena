using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fornecedor.Query.ListarFornecedores;

public sealed class ListarFornecedoresQueryValidation : AbstractValidator<ListarFornecedoresQuery>
{
    public ListarFornecedoresQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
        RuleFor(x => x.Termo).MaximumLength(100);
    }
}
