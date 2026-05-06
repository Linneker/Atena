using FluentValidation;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.ExportarLogs;

public sealed class ExportarLogsQueryValidation : AbstractValidator<ExportarLogsQuery>
{
    public ExportarLogsQueryValidation()
    {
        RuleFor(x => x.Entidade).MaximumLength(100);
        When(x => x.Inicio.HasValue && x.Fim.HasValue, () =>
        {
            RuleFor(x => x.Fim).GreaterThanOrEqualTo(x => x.Inicio);
        });
    }
}
