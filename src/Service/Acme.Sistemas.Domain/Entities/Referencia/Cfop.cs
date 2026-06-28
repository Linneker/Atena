namespace Acme.Sistemas.Domain.Entities.Referencia;

/// <summary>
/// Código Fiscal de Operações e Prestações. Catálogo de referência nacional (não tenant-scoped).
/// Categoria: "Entrada" (1xxx/2xxx/3xxx) ou "Saida" (5xxx/6xxx/7xxx).
/// </summary>
public sealed class Cfop
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int SeedVersion { get; set; }
}
