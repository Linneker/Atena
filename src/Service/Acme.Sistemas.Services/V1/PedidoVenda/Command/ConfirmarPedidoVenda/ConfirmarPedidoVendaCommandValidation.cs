using FluentValidation;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Command.ConfirmarPedidoVenda;

public sealed class ConfirmarPedidoVendaCommandValidation : AbstractValidator<ConfirmarPedidoVendaCommand>
{
    public ConfirmarPedidoVendaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
