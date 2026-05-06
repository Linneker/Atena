namespace Acme.Sistemas.Domain.Entities.Vendas;

public enum TipoFaturamento
{
    Total = 1,
    Parcial = 2
}

public sealed class Faturamento : BaseEntity
{
    public string Numero { get; set; } = string.Empty;
    public Guid PedidoVendaId { get; set; }
    public DateTime DataFaturamento { get; set; } = DateTime.UtcNow;
    public TipoFaturamento Tipo { get; set; } = TipoFaturamento.Total;
    public decimal ValorTotal { get; set; }
    public Guid? NFeId { get; set; }
    public Guid? ContaReceberId { get; set; }
    public string? Observacao { get; set; }
    public List<FaturamentoItem> Itens { get; set; } = new();
}

public sealed class FaturamentoItem : BaseEntity
{
    public Guid FaturamentoId { get; set; }
    public Guid PedidoVendaItemId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}
