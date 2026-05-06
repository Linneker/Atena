using FluentValidation;

namespace Acme.Sistemas.Services.V1.RecebimentoCompra.Command.RegistrarRecebimento;

public sealed class RegistrarRecebimentoCommandValidation : AbstractValidator<RegistrarRecebimentoCommand>
{
    public RegistrarRecebimentoCommandValidation()
    {
        RuleFor(x => x.PedidoCompraId).NotEmpty();
        RuleFor(x => x.EstoqueId).NotEmpty();
        RuleFor(x => x.VencimentoContaPagar).NotEmpty();
        RuleFor(x => x.Itens).NotEmpty();
        RuleForEach(x => x.Itens).ChildRules(i =>
        {
            i.RuleFor(x => x.PedidoCompraItemId).NotEmpty();
            i.RuleFor(x => x.QuantidadeRecebida).GreaterThan(0);
            i.RuleFor(x => x.PrecoUnitario).GreaterThanOrEqualTo(0).When(x => x.PrecoUnitario.HasValue);
        });
        RuleFor(x => x.NumeroNotaFiscal).MaximumLength(30);
        RuleFor(x => x.ChaveAcessoNFe).Length(44).When(x => !string.IsNullOrWhiteSpace(x.ChaveAcessoNFe));
    }
}
