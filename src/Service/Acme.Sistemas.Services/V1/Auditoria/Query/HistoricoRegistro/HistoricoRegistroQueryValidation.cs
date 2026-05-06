using FluentValidation;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.HistoricoRegistro;

public sealed class HistoricoRegistroQueryValidation : AbstractValidator<HistoricoRegistroQuery>
{
    public HistoricoRegistroQueryValidation()
    {
        RuleFor(x => x.Entidade).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EntidadeId).NotEmpty();
    }
}
