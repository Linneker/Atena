using FluentValidation;

namespace Acme.Sistemas.Services.V1.Relatorios.Financeiro.DRE;

public sealed class GerarDREQueryValidation : AbstractValidator<GerarDREQuery>
{
    public GerarDREQueryValidation()
    {
        RuleFor(x => x.Inicio).NotEmpty();
        RuleFor(x => x.Fim).NotEmpty()
            .GreaterThanOrEqualTo(x => x.Inicio)
            .WithMessage("Fim deve ser >= Inicio.");
    }
}
