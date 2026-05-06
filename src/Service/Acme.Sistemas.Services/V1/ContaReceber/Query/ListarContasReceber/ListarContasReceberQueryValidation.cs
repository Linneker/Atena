using FluentValidation;

namespace Acme.Sistemas.Services.V1.ContaReceber.Query.ListarContasReceber;

public sealed class ListarContasReceberQueryValidation : AbstractValidator<ListarContasReceberQuery>
{
    public ListarContasReceberQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
        RuleFor(x => x.DiasAtrasoMinimo).GreaterThanOrEqualTo(0).When(x => x.DiasAtrasoMinimo.HasValue);
    }
}
