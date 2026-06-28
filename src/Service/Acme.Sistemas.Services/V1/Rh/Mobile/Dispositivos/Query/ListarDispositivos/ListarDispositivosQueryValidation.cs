using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Query.ListarDispositivos;

public sealed class ListarDispositivosQueryValidation : AbstractValidator<ListarDispositivosQuery>
{
    public ListarDispositivosQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
