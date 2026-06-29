namespace Acme.Sistemas.Domain.Entities.Referencia.Rh;

/// <summary>
/// Rubrica modelo do catálogo nacional. Read-only. Tenant clona para a sua <c>rubricas_tenant</c>
/// e ajusta. Chave natural = código. <c>DependenciasJson</c> lista códigos de outras rubricas que
/// a fórmula referencia via <c>vlr['CODIGO']</c>.
/// </summary>
public sealed class RubricaCatalogoNacional
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string? NaturezaEsocialCodigo { get; set; }
    public string FormulaDsl { get; set; } = string.Empty;
    public bool IncideInss { get; set; }
    public bool IncideIrrf { get; set; }
    public bool IncideFgts { get; set; }
    public bool IncideFerias { get; set; }
    public bool Incide13o { get; set; }
    public bool IncideDsr { get; set; }
    public string? DependenciasJson { get; set; }
    public string SeedOrigem { get; set; } = "migration";
    public DateTime ImportadoEm { get; set; }
}
