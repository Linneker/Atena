namespace Acme.Sistemas.Domain.Entities.Referencia.Rh;

/// <summary>
/// Natureza de rubrica do eSocial (evento S-1010 — Tabela de Rubricas). Códigos oficiais:
/// 1xxx/2xxx/3xxx = proventos; 5xxx = informativas; 9xxx = descontos. Chave natural = código.
/// </summary>
public sealed class NaturezaRubricaEsocial
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string TipoGrupo { get; set; } = string.Empty;
    public bool Ativa { get; set; } = true;
    public string SeedOrigem { get; set; } = "migration";
    public DateTime ImportadoEm { get; set; }
}
