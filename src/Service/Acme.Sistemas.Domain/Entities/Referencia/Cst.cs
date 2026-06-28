namespace Acme.Sistemas.Domain.Entities.Referencia;

/// <summary>
/// Código de Situação Tributária. Catálogo de referência nacional (não tenant-scoped).
/// O <see cref="Tipo"/> identifica o imposto: icms | pis | cofins | ipi.
/// </summary>
public sealed class Cst
{
    public string Tipo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}
