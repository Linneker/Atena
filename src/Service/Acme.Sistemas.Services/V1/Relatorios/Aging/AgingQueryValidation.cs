using FluentValidation;

namespace Acme.Sistemas.Services.V1.Relatorios.Aging;

public sealed class AgingQueryValidation : AbstractValidator<AgingQuery>
{
    public AgingQueryValidation()
    {
        RuleFor(x => x.Tipo).IsInEnum();
    }
}
