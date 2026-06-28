using FluentValidation;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Query.ListarPedidosVenda;

public sealed class ListarPedidosVendaQueryValidation : AbstractValidator<ListarPedidosVendaQuery>
{
    public ListarPedidosVendaQueryValidation()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
