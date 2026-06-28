using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Command.FecharCompetencia;

public sealed class FecharCompetenciaCommandValidation : AbstractValidator<FecharCompetenciaCommand>
{
    public FecharCompetenciaCommandValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.Competencia).NotEmpty().Matches(@"^\d{4}-\d{2}$");
    }
}
