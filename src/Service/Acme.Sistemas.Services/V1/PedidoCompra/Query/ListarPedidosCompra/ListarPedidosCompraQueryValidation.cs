using FluentValidation;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Query.ListarPedidosCompra;

public sealed class ListarPedidosCompraQueryValidation : AbstractValidator<ListarPedidosCompraQuery>
{
    public ListarPedidosCompraQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
