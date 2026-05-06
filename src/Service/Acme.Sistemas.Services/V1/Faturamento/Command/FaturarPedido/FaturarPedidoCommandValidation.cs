using FluentValidation;

namespace Acme.Sistemas.Services.V1.Faturamento.Command.FaturarPedido;

public sealed class FaturarPedidoCommandValidation : AbstractValidator<FaturarPedidoCommand>
{
    public FaturarPedidoCommandValidation()
    {
        RuleFor(x => x.PedidoVendaId).NotEmpty();
        RuleFor(x => x.VencimentoContaReceber).NotEmpty();
        RuleFor(x => x.PercentualComissaoOverride).InclusiveBetween(0, 100)
            .When(x => x.PercentualComissaoOverride.HasValue);
        RuleFor(x => x.Itens).NotEmpty();
        RuleForEach(x => x.Itens).ChildRules(i =>
        {
            i.RuleFor(x => x.PedidoVendaItemId).NotEmpty();
            i.RuleFor(x => x.Quantidade).GreaterThan(0);
        });
    }
}
