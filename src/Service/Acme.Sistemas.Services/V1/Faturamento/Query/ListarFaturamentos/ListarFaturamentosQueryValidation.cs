using FluentValidation;

namespace Acme.Sistemas.Services.V1.Faturamento.Query.ListarFaturamentos;

public sealed class ListarFaturamentosQueryValidation : AbstractValidator<ListarFaturamentosQuery>
{
    public ListarFaturamentosQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
