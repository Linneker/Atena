using FluentValidation;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Query.ObterPedidoVenda;

public sealed class ObterPedidoVendaQueryValidation : AbstractValidator<ObterPedidoVendaQuery>
{
    public ObterPedidoVendaQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
