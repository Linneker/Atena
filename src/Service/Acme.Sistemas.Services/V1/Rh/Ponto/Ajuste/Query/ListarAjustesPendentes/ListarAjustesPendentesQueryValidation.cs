using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Query.ListarAjustesPendentes;

public sealed class ListarAjustesPendentesQueryValidation : AbstractValidator<ListarAjustesPendentesQuery>
{
    public ListarAjustesPendentesQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
