using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.IncluirMarcacaoManual;

public sealed class IncluirMarcacaoManualCommandValidation : AbstractValidator<IncluirMarcacaoManualCommand>
{
    public IncluirMarcacaoManualCommandValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.Motivo).NotEmpty().MaximumLength(500);
        RuleFor(x => x.DataHora).Must(d => d <= DateTime.UtcNow.AddMinutes(1))
            .WithMessage("dataHora não pode ser no futuro.");
    }
}
