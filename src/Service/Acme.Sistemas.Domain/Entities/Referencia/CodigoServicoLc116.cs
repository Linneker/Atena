namespace Acme.Sistemas.Domain.Entities.Referencia;

/// <summary>
/// Código de serviço da Lei Complementar 116/2003 (lista nacional de serviços para ISSQN/NFS-e).
/// Catálogo de referência nacional (não tenant-scoped).
/// </summary>
public sealed class CodigoServicoLc116
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}
