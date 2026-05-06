using FluentValidation;

namespace Acme.Sistemas.Services.V1.Dashboard.Query.ObterKpis;

public sealed class ObterKpisQueryValidation : AbstractValidator<ObterKpisQuery>
{
    public ObterKpisQueryValidation()
    {
        When(x => x.Inicio.HasValue && x.Fim.HasValue, () =>
        {
            RuleFor(x => x.Fim).GreaterThanOrEqualTo(x => x.Inicio);
        });
    }
}
