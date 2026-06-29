using Acme.Sistemas.Domain.Enums.Rh;

namespace Acme.Sistemas.Domain.Entities.Rh.Oficial671;

/// <summary>Metadados de uma exportação AEJ (Portaria 671 anexo IV) — JSON + JWS em S3.</summary>
public sealed class ExportacaoAej : BaseEntity
{
    public Guid EmpresaId { get; set; }
    public DateOnly PeriodoInicio { get; set; }
    public DateOnly PeriodoFim { get; set; }
    public string LayoutVersao { get; set; } = "v1";
    public string? ArquivoUrl { get; set; }
    public string? AssinaturaUrl { get; set; }
    public string? HashSha256 { get; set; }
    public StatusExportacao671 Status { get; set; } = StatusExportacao671.Solicitada;
    public DateTime? GeradoEm { get; set; }
    public string? Erro { get; set; }
}
