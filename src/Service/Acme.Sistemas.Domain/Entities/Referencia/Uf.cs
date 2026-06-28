namespace Acme.Sistemas.Domain.Entities.Referencia;

/// <summary>
/// Unidade Federativa brasileira. Catálogo de referência nacional (não tenant-scoped),
/// semeado por migration. Chave natural = sigla.
/// </summary>
public sealed class Uf
{
    public string Sigla { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int CodigoIbge { get; set; }
}
