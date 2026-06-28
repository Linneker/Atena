using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class FechamentoPonto : BaseEntity
{
    public Guid FuncionarioId { get; set; }
    /// <summary>YYYY-MM.</summary>
    public string Competencia { get; set; } = string.Empty;
    public StatusFechamentoPonto Status { get; set; } = StatusFechamentoPonto.Aberto;
    public DateTime? FechadoEm { get; set; }
    public Guid? FechadoPor { get; set; }
    public DateTime? ReabertoEm { get; set; }
    public Guid? ReabertoPor { get; set; }
    public string? MotivoReabertura { get; set; }
    public string? EspelhoUrl { get; set; }
    public string? EspelhoHash { get; set; }
    public string? Observacoes { get; set; }
}
