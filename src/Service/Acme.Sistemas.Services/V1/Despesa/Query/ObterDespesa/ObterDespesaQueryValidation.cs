using FluentValidation;

namespace Acme.Sistemas.Services.V1.Despesa.Query.ObterDespesa;

public sealed class ObterDespesaQueryValidation : AbstractValidator<ObterDespesaQuery>
{
    public ObterDespesaQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
