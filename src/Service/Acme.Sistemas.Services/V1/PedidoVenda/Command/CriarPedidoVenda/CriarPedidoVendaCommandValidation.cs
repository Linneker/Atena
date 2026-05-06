using FluentValidation;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Command.CriarPedidoVenda;

public sealed class CriarPedidoVendaCommandValidation : AbstractValidator<CriarPedidoVendaCommand>
{
    public CriarPedidoVendaCommandValidation()
    {
        RuleFor(x => x.ClienteId).NotEmpty();
        RuleFor(x => x.EstoqueId).NotEmpty();
        RuleFor(x => x.DescontoPercentual).InclusiveBetween(0, 100).When(x => x.DescontoPercentual.HasValue);
        RuleFor(x => x.Itens).NotEmpty();
        RuleForEach(x => x.Itens).ChildRules(i =>
        {
            i.RuleFor(x => x.ProdutoId).NotEmpty();
            i.RuleFor(x => x.Quantidade).GreaterThan(0);
            i.RuleFor(x => x.PrecoUnitario).GreaterThan(0);
        });
    }
}
