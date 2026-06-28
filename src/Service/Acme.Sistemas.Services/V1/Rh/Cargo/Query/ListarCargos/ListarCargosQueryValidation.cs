using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Query.ListarCargos;

public sealed class ListarCargosQueryValidation : AbstractValidator<ListarCargosQuery>
{
    public ListarCargosQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
