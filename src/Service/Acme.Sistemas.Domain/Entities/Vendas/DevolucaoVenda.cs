namespace Acme.Sistemas.Domain.Entities.Vendas;

public enum TipoDevolucao
{
    Total = 1,
    Parcial = 2
}

public sealed class DevolucaoVenda : BaseEntity
{
    public Guid FaturamentoId { get; set; }
    public DateTime DataDevolucao { get; set; } = DateTime.UtcNow;
    public TipoDevolucao Tipo { get; set; } = TipoDevolucao.Total;
    public decimal ValorTotal { get; set; }
    public string? Motivo { get; set; }
    public Guid? NFeDevolucaoId { get; set; }
    public List<DevolucaoVendaItem> Itens { get; set; } = new();
}

public sealed class DevolucaoVendaItem : BaseEntity
{
    public Guid DevolucaoVendaId { get; set; }
    public Guid FaturamentoItemId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}
