namespace Acme.Sistemas.Domain.Entities.Referencia.Rh;

/// <summary>
/// Alíquotas FGTS vigentes por competência. <c>AliquotaContribuicaoSocialPct</c> (LC 110/2001)
/// mantida como coluna para auditoria — atualmente suspensa (= 0). Aprendiz tem alíquota reduzida (2%).
/// </summary>
public sealed class TabelaFgts
{
    public Guid Id { get; set; }
    public string CompetenciaInicio { get; set; } = string.Empty;
    public string? CompetenciaFim { get; set; }
    public decimal AliquotaNormalPct { get; set; }
    public decimal AliquotaMultaRescisaoPct { get; set; }
    public decimal AliquotaContribuicaoSocialPct { get; set; }
    public decimal AliquotaAprendizPct { get; set; }
    public string SeedOrigem { get; set; } = "migration";
    public DateTime ImportadoEm { get; set; }
    public Guid? ImportadoPor { get; set; }
}
