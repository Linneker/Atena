using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Query.ListarMarcacoesPorPeriodo;

public sealed class ListarMarcacoesPorPeriodoQueryValidation : AbstractValidator<ListarMarcacoesPorPeriodoQuery>
{
    public ListarMarcacoesPorPeriodoQueryValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x).Must(q => q.DataFim >= q.DataInicio)
            .WithMessage("dataFim deve ser >= dataInicio.");
        RuleFor(x => x).Must(q => (q.DataFim.DayNumber - q.DataInicio.DayNumber) <= 366)
            .WithMessage("intervalo máximo de 366 dias.");
    }
}
