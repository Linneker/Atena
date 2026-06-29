namespace Acme.Sistemas.Domain.Entities.Referencia.Rh;

/// <summary>
/// Uma faixa progressiva da tabela INSS vigente por competência (modelo pós-Reforma 2019).
/// Catálogo nacional, não tenant-scoped. <c>CompetenciaFim=null</c> indica vigência aberta.
/// </summary>
public sealed class TabelaInssFaixa
{
    public Guid Id { get; set; }
    public string CompetenciaInicio { get; set; } = string.Empty;
    public string? CompetenciaFim { get; set; }
    public byte OrdemFaixa { get; set; }
    public decimal FaixaInicio { get; set; }
    public decimal FaixaFim { get; set; }
    public decimal AliquotaPct { get; set; }
    public decimal ParcelaDeduzir { get; set; }
    public string SeedOrigem { get; set; } = "migration";
    public DateTime ImportadoEm { get; set; }
    public Guid? ImportadoPor { get; set; }
}
