using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Tenants;

public sealed class Tenant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string RazaoSocial { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Plano { get; set; } = Constants.TenantPlano.Free;
    public StatusAtivo Status { get; set; } = StatusAtivo.Ativo;
    public string? LogoUrl { get; set; }
    public string? CorPrimaria { get; set; }
    public string FusoHorario { get; set; } = "America/Sao_Paulo";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
