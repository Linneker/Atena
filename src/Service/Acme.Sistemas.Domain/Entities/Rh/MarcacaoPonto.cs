using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class MarcacaoPonto : BaseEntity
{
    public Guid FuncionarioId { get; set; }
    public TipoMarcacao Tipo { get; set; }
    public DateTime DataHora { get; set; }
    public OrigemMarcacao Origem { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? IpOrigem { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceId { get; set; }
    public string? FotoUrl { get; set; }
    public string? HashAnterior { get; set; }
    public string HashIntegridade { get; set; } = string.Empty;
    public StatusMarcacao Status { get; set; } = StatusMarcacao.Valida;
    public Guid? MarcacaoOrigemId { get; set; }
}
