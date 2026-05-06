using FluentValidation;

namespace Acme.Sistemas.Services.V1.Tenant.Command.AlterarTenant;

public sealed class AlterarTenantCommandValidation : AbstractValidator<AlterarTenantCommand>
{
    public AlterarTenantCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RazaoSocial).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Plano).NotEmpty();
        RuleFor(x => x.Status).InclusiveBetween(0, 2);
        RuleFor(x => x.FusoHorario).NotEmpty().MaximumLength(50);
    }
}
