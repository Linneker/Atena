namespace Acme.Sistemas.Domain.Entities.Referencia.Rh;

/// <summary>
/// Uma faixa progressiva da tabela IRRF mensal vigente por competência. Inclui o valor
/// de dedução por dependente e a dedução simplificada (Lei 14.848/2024) — comuns à vigência inteira,
/// repetidos em todas as faixas para facilitar consulta.
/// </summary>
public sealed class TabelaIrrfFaixa
{
    public Guid Id { get; set; }
    public string CompetenciaInicio { get; set; } = string.Empty;
    public string? CompetenciaFim { get; set; }
    public byte OrdemFaixa { get; set; }
    public decimal FaixaInicio { get; set; }
    public decimal FaixaFim { get; set; }
    public decimal AliquotaPct { get; set; }
    public decimal ParcelaDeduzir { get; set; }
    public decimal DeducaoPorDependente { get; set; }
    public decimal DeducaoSimplificada { get; set; }
    public string SeedOrigem { get; set; } = "migration";
    public DateTime ImportadoEm { get; set; }
    public Guid? ImportadoPor { get; set; }
}
