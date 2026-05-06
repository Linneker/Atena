using FluentValidation;

namespace Acme.Sistemas.Services.V1.Cliente.Query.ListarClientes;

public sealed class ListarClientesQueryValidation : AbstractValidator<ListarClientesQuery>
{
    public ListarClientesQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
        RuleFor(x => x.Termo).MaximumLength(100);
    }
}
