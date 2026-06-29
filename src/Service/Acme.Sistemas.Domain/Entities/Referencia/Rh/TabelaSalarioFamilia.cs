namespace Acme.Sistemas.Domain.Entities.Referencia.Rh;

/// <summary>
/// Salário-família — Lei 4.266/63 e Portaria Interministerial anual. Funcionários com remuneração
/// até <c>LimiteRemuneracao</c> recebem <c>ValorCota</c> por dependente legal (filho/filha &lt; 14
/// anos ou inválido). Vigência por competência.
/// </summary>
public sealed class TabelaSalarioFamilia
{
    public Guid Id { get; set; }
    public string CompetenciaInicio { get; set; } = string.Empty;
    public string? CompetenciaFim { get; set; }
    public decimal LimiteRemuneracao { get; set; }
    public decimal ValorCota { get; set; }
    public string SeedOrigem { get; set; } = "migration";
    public DateTime ImportadoEm { get; set; }
    public Guid? ImportadoPor { get; set; }
}
