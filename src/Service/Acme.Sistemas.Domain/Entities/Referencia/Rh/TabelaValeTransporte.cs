namespace Acme.Sistemas.Domain.Entities.Referencia.Rh;

/// <summary>
/// Regra do vale-transporte — Lei 7.418/85 e Decreto 95.247/87 (art. 9). Desconto máximo de
/// 6% do salário-base. Persistido para auditoria e configurabilidade futura (apesar de a regra
/// ser nacional fixa, fica versionado em caso de alteração legal).
/// </summary>
public sealed class TabelaValeTransporte
{
    public Guid Id { get; set; }
    public string CompetenciaInicio { get; set; } = string.Empty;
    public string? CompetenciaFim { get; set; }
    public decimal DescontoMaxPct { get; set; }
    public string? AtoLegal { get; set; }
    public string SeedOrigem { get; set; } = "migration";
    public DateTime ImportadoEm { get; set; }
    public Guid? ImportadoPor { get; set; }
}
