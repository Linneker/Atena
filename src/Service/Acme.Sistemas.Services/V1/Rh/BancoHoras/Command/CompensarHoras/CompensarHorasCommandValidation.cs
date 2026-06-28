using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.CompensarHoras;

public sealed class CompensarHorasCommandValidation : AbstractValidator<CompensarHorasCommand>
{
    public CompensarHorasCommandValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.Minutos).GreaterThan(0).WithMessage("Minutos a compensar deve ser positivo.");
        RuleFor(x => x.Motivo).NotEmpty().MaximumLength(500);
    }
}
