using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Command.ExportarAej;

public sealed class ExportarAejCommandValidation : AbstractValidator<ExportarAejCommand>
{
    public ExportarAejCommandValidation()
    {
        RuleFor(x => x.EmpresaId).NotEmpty();
        RuleFor(x => x.PeriodoInicio).LessThanOrEqualTo(x => x.PeriodoFim);
        RuleFor(x => x.PeriodoFim).Must((cmd, fim) =>
            (fim.DayNumber - cmd.PeriodoInicio.DayNumber) <= 366)
            .WithMessage("Período não pode exceder 366 dias.");
    }
}
