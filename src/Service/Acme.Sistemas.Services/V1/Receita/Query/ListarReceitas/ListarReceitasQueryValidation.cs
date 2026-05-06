using FluentValidation;

namespace Acme.Sistemas.Services.V1.Receita.Query.ListarReceitas;

public sealed class ListarReceitasQueryValidation : AbstractValidator<ListarReceitasQuery>
{
    public ListarReceitasQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
        RuleFor(x => x.Categoria).MaximumLength(100);
        When(x => x.RecebimentoInicio.HasValue && x.RecebimentoFim.HasValue, () =>
        {
            RuleFor(x => x.RecebimentoFim)
                .GreaterThanOrEqualTo(x => x.RecebimentoInicio)
                .WithMessage("RecebimentoFim deve ser maior ou igual a RecebimentoInicio.");
        });
    }
}
