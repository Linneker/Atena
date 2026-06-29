namespace Acme.Sistemas.Domain.Entities.Referencia.Rh;

/// <summary>
/// Salário-mínimo federal vigente por competência. <c>AtoLegal</c> mantém a referência da
/// MP/Decreto que o instituiu (auditoria de procedência).
/// </summary>
public sealed class SalarioMinimoNacional
{
    public Guid Id { get; set; }
    public string CompetenciaInicio { get; set; } = string.Empty;
    public string? CompetenciaFim { get; set; }
    public decimal Valor { get; set; }
    public string? AtoLegal { get; set; }
    public string SeedOrigem { get; set; } = "migration";
    public DateTime ImportadoEm { get; set; }
    public Guid? ImportadoPor { get; set; }
}
