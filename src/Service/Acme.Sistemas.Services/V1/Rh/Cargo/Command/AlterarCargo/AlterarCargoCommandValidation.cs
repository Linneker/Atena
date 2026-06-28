using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Command.AlterarCargo;

public sealed class AlterarCargoCommandValidation : AbstractValidator<AlterarCargoCommand>
{
    public AlterarCargoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Codigo).MaximumLength(20);
        RuleFor(x => x.CodigoCbo).Matches(@"^\d{6}$")
            .When(x => !string.IsNullOrWhiteSpace(x.CodigoCbo))
            .WithMessage("codigoCbo deve ter exatamente 6 dígitos numéricos.");
        RuleFor(x => x.SalarioBaseSugerido).GreaterThan(0)
            .When(x => x.SalarioBaseSugerido.HasValue);
    }
}
