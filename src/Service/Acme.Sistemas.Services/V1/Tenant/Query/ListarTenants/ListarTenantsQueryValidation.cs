using FluentValidation;

namespace Acme.Sistemas.Services.V1.Tenant.Query.ListarTenants;

public sealed class ListarTenantsQueryValidation : AbstractValidator<ListarTenantsQuery>
{
    public ListarTenantsQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
