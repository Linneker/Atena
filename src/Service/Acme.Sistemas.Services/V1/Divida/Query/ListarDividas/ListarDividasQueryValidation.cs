using FluentValidation;

namespace Acme.Sistemas.Services.V1.Divida.Query.ListarDividas;

public sealed class ListarDividasQueryValidation : AbstractValidator<ListarDividasQuery>
{
    public ListarDividasQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
