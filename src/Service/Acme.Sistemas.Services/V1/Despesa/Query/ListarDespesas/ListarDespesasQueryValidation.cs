using FluentValidation;

namespace Acme.Sistemas.Services.V1.Despesa.Query.ListarDespesas;

public sealed class ListarDespesasQueryValidation : AbstractValidator<ListarDespesasQuery>
{
    public ListarDespesasQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
        RuleFor(x => x.Categoria).MaximumLength(100);
        When(x => x.VencimentoInicio.HasValue && x.VencimentoFim.HasValue, () =>
        {
            RuleFor(x => x.VencimentoFim)
                .GreaterThanOrEqualTo(x => x.VencimentoInicio)
                .WithMessage("VencimentoFim deve ser maior ou igual a VencimentoInicio.");
        });
    }
}
