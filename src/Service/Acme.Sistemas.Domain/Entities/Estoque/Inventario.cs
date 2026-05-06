namespace Acme.Sistemas.Domain.Entities.Estoque;

public enum StatusInventario
{
    Aberto = 0,
    EmContagem = 1,
    Fechado = 2,
    Cancelado = 3
}

public sealed class Inventario : BaseEntity
{
    public Guid EstoqueId { get; set; }
    public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
    public DateTime? DataFechamento { get; set; }
    public StatusInventario Status { get; set; } = StatusInventario.Aberto;
    public string? Observacao { get; set; }
}

public sealed class InventarioItem : BaseEntity
{
    public Guid InventarioId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal SaldoSistema { get; set; }
    public decimal? SaldoContado { get; set; }
    public decimal Diferenca => (SaldoContado ?? 0) - SaldoSistema;
    public string? Observacao { get; set; }
}
