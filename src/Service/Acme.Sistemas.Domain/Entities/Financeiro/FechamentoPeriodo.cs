namespace Acme.Sistemas.Domain.Entities.Financeiro;

public sealed class FechamentoPeriodo : BaseEntity
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public DateTime FechadoEm { get; set; } = DateTime.UtcNow;
    public Guid? FechadoPor { get; set; }
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Resultado { get; set; }
    public string? Observacao { get; set; }
}
