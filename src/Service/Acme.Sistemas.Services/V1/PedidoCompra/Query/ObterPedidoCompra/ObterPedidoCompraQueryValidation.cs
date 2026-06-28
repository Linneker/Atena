using FluentValidation;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Query.ObterPedidoCompra;

public sealed class ObterPedidoCompraQueryValidation : AbstractValidator<ObterPedidoCompraQuery>
{
    public ObterPedidoCompraQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
