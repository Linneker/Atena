using FluentValidation;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Query.ListarCentrosDeCusto;

public sealed class ListarCentrosDeCustoQueryValidation : AbstractValidator<ListarCentrosDeCustoQuery>
{
    public ListarCentrosDeCustoQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 500);
    }
}
