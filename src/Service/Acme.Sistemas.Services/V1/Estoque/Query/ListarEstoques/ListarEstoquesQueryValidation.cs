using FluentValidation;

namespace Acme.Sistemas.Services.V1.Estoque.Query.ListarEstoques;

public sealed class ListarEstoquesQueryValidation : AbstractValidator<ListarEstoquesQuery>
{
    public ListarEstoquesQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
