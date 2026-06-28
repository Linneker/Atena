using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Command.ReabrirCompetencia;

public sealed class ReabrirCompetenciaCommandValidation : AbstractValidator<ReabrirCompetenciaCommand>
{
    public ReabrirCompetenciaCommandValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.Competencia).NotEmpty().Matches(@"^\d{4}-\d{2}$");
        RuleFor(x => x.Motivo).NotEmpty().MaximumLength(2000);
    }
}
