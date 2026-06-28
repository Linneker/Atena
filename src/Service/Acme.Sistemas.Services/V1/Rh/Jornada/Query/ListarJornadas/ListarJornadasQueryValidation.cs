using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Query.ListarJornadas;

public sealed class ListarJornadasQueryValidation : AbstractValidator<ListarJornadasQuery>
{
    public ListarJornadasQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
