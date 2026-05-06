using FluentValidation;

namespace Acme.Sistemas.Services.V1.Tenant.Query.ObterTenant;

public sealed class ObterTenantQueryValidation : AbstractValidator<ObterTenantQuery>
{
    public ObterTenantQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
