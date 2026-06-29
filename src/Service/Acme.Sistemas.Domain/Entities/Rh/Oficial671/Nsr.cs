namespace Acme.Sistemas.Domain.Entities.Rh.Oficial671;

/// <summary>
/// Numerador atômico de NSR por (tenant, empresa). Tabela <c>numerador_nsr</c>.
/// </summary>
public sealed class Nsr : BaseEntity
{
    public Guid EmpresaId { get; set; }
    public long UltimoNumero { get; set; }
    public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
}
