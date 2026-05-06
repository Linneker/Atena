namespace Acme.Sistemas.Domain.Entities.Vendas;

public enum StatusPedidoVenda
{
    Rascunho = 0,
    Confirmado = 1,
    FaturamentoParcial = 2,
    Faturado = 3,
    Cancelado = 4
}

public sealed class PedidoVenda : BaseEntity
{
    public string Numero { get; set; } = string.Empty;
    public Guid ClienteId { get; set; }
    public Guid? VendedorId { get; set; }
    public Guid? OrcamentoId { get; set; }
    public DateTime DataEmissao { get; set; } = DateTime.UtcNow;
    public Guid EstoqueId { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal? DescontoPercentual { get; set; }
    public StatusPedidoVenda Status { get; set; } = StatusPedidoVenda.Rascunho;
    public string? CondicaoPagamento { get; set; }
    public string? Observacao { get; set; }
    public List<PedidoVendaItem> Itens { get; set; } = new();
}

public sealed class PedidoVendaItem : BaseEntity
{
    public Guid PedidoVendaId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal QuantidadeFaturada { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Total => Quantidade * PrecoUnitario;
}
