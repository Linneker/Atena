using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class AjustePonto : BaseEntity
{
    public Guid FuncionarioId { get; set; }
    public Guid? MarcacaoOriginalId { get; set; }
    public TipoAjuste TipoAjuste { get; set; }
    public DateTime? DataHoraProposta { get; set; }
    public TipoMarcacao? TipoMarcacaoProposta { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string? AnexoUrl { get; set; }
    public StatusAjuste Status { get; set; } = StatusAjuste.Pendente;
    public Guid? AprovadorId { get; set; }
    public DateTime? DecisaoEm { get; set; }
    public string? JustificativaDecisao { get; set; }
    public Guid? MarcacaoResultanteId { get; set; }
}
