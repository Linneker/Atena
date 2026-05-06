namespace Acme.Sistemas.Domain.Entities.Compras;

public enum TipoRecebimento
{
    Total = 1,
    Parcial = 2,
    ComDivergencia = 3
}

public sealed class RecebimentoCompra : BaseEntity
{
    public Guid PedidoCompraId { get; set; }
    public DateTime DataRecebimento { get; set; } = DateTime.UtcNow;
    public TipoRecebimento Tipo { get; set; } = TipoRecebimento.Total;
    public string? NumeroNotaFiscal { get; set; }
    public string? ChaveAcessoNFe { get; set; }
    public string? Observacao { get; set; }
    public List<RecebimentoCompraItem> Itens { get; set; } = new();
}

public sealed class RecebimentoCompraItem : BaseEntity
{
    public Guid RecebimentoCompraId { get; set; }
    public Guid PedidoCompraItemId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal QuantidadeRecebida { get; set; }
    public decimal? PrecoUnitario { get; set; }
    public string? Observacao { get; set; }
}
