using FluentValidation;

namespace Acme.Sistemas.Services.V1.FluxoDeCaixa.Command.FecharPeriodo;

public sealed class FecharPeriodoCommandValidation : AbstractValidator<FecharPeriodoCommand>
{
    public FecharPeriodoCommandValidation()
    {
        RuleFor(x => x.Ano).InclusiveBetween(2000, 2100);
        RuleFor(x => x.Mes).InclusiveBetween(1, 12);
        RuleFor(x => x.Observacao).MaximumLength(500);
    }
}
