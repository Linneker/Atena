namespace Acme.Sistemas.Domain.Entities.Auditoria;

public enum OperacaoAuditoria
{
    Criar = 1,
    Alterar = 2,
    Excluir = 3,
    Outro = 99
}

public sealed class AuditLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public string EntidadeNome { get; set; } = string.Empty;
    public Guid? EntidadeId { get; set; }
    public OperacaoAuditoria Operacao { get; set; }
    public string CommandTipo { get; set; } = string.Empty;
    public string? AntesJson { get; set; }
    public string? DepoisJson { get; set; }
    public DateTime OcorridoEm { get; set; } = DateTime.UtcNow;
}

public sealed class ApiRequestAudit : BaseEntity
{
    public Guid? UserId { get; set; }
    public string Metodo { get; set; } = string.Empty;
    public string Caminho { get; set; } = string.Empty;
    public string? QueryString { get; set; }
    public int StatusCode { get; set; }
    public long DuracaoMs { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? CorrelationId { get; set; }
    public DateTime OcorridoEm { get; set; } = DateTime.UtcNow;
}
