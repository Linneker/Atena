using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarPoliticas;

public sealed class ListarPoliticasQueryValidation : AbstractValidator<ListarPoliticasQuery>
{
    public ListarPoliticasQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
