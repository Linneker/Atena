namespace Acme.Sistemas.Domain.Entities.Compras;

public enum StatusPedidoCompra
{
    Rascunho = 0,
    EnviadoFornecedor = 1,
    ConfirmadoFornecedor = 2,
    RecebimentoParcial = 3,
    Recebido = 4,
    Cancelado = 5
}

public sealed class PedidoCompra : BaseEntity
{
    public string Numero { get; set; } = string.Empty;
    public Guid FornecedorId { get; set; }
    public Guid? SolicitacaoCompraId { get; set; }
    public DateTime DataEmissao { get; set; } = DateTime.UtcNow;
    public DateTime? PrevisaoEntrega { get; set; }
    public string? CondicaoPagamento { get; set; }
    public decimal ValorTotal { get; set; }
    public StatusPedidoCompra Status { get; set; } = StatusPedidoCompra.Rascunho;
    public string? Observacao { get; set; }
    public List<PedidoCompraItem> Itens { get; set; } = new();
}

public sealed class PedidoCompraItem : BaseEntity
{
    public Guid PedidoCompraId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal QuantidadeRecebida { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Total => Quantidade * PrecoUnitario;
}
