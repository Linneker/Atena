namespace Acme.Sistemas.Domain.Entities.Referencia;

/// <summary>
/// Classificação Brasileira de Ocupações (CBO). Catálogo de referência nacional
/// (não tenant-scoped). Tabela semeada via endpoint admin opt-in. Chave natural = código (6 dígitos).
/// </summary>
public sealed class Cbo
{
    public string Codigo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? GrandeGrupo { get; set; }
    public string? Familia { get; set; }
    public bool Ativo { get; set; } = true;
}
