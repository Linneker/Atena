using FluentValidation;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.ListarLogs;

public sealed class ListarLogsQueryValidation : AbstractValidator<ListarLogsQuery>
{
    public ListarLogsQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 500);
        RuleFor(x => x.Entidade).MaximumLength(100);
    }
}
