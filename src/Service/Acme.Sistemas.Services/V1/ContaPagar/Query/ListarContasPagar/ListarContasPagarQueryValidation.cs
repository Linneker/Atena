using FluentValidation;

namespace Acme.Sistemas.Services.V1.ContaPagar.Query.ListarContasPagar;

public sealed class ListarContasPagarQueryValidation : AbstractValidator<ListarContasPagarQuery>
{
    public ListarContasPagarQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
