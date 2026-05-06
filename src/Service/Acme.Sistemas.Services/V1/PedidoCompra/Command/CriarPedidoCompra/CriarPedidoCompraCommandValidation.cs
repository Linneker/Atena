using FluentValidation;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Command.CriarPedidoCompra;

public sealed class CriarPedidoCompraCommandValidation : AbstractValidator<CriarPedidoCompraCommand>
{
    public CriarPedidoCompraCommandValidation()
    {
        RuleFor(x => x.FornecedorId).NotEmpty();
        RuleFor(x => x.CondicaoPagamento).MaximumLength(100);
        RuleFor(x => x.Observacao).MaximumLength(2000);

        // Itens obrigatórios se SolicitacaoCompraId não for fornecida
        When(x => !x.SolicitacaoCompraId.HasValue, () =>
        {
            RuleFor(x => x.Itens).NotEmpty()
                .WithMessage("Para pedido direto (sem solicitação) é obrigatório informar itens.");
        });

        RuleForEach(x => x.Itens!).ChildRules(i =>
        {
            i.RuleFor(x => x.ProdutoId).NotEmpty();
            i.RuleFor(x => x.Quantidade).GreaterThan(0);
            i.RuleFor(x => x.PrecoUnitario).GreaterThan(0);
        }).When(x => x.Itens is not null && x.Itens.Count > 0);
    }
}
