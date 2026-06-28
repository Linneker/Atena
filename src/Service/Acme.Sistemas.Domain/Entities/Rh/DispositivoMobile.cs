using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class DispositivoMobile : BaseEntity
{
    public Guid? FuncionarioId { get; set; }
    public Guid UsuarioId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public PlataformaMobile Plataforma { get; set; }
    public string? Modelo { get; set; }
    public string? OsVersion { get; set; }
    public string? AppVersion { get; set; }
    public string? PushToken { get; set; }
    public string? ChavePublicaLocal { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime? RevogadoEm { get; set; }
    public Guid? RevogadoPor { get; set; }
    public DateTime RegistradoEm { get; set; } = DateTime.UtcNow;
    public DateTime? UltimoAcesso { get; set; }
}
