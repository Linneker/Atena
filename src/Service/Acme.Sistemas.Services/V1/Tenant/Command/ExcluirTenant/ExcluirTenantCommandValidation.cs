using FluentValidation;

namespace Acme.Sistemas.Services.V1.Tenant.Command.ExcluirTenant;

public sealed class ExcluirTenantCommandValidation : AbstractValidator<ExcluirTenantCommand>
{
    public ExcluirTenantCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
