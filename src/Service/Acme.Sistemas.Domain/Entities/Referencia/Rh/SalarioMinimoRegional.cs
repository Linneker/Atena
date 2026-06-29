namespace Acme.Sistemas.Domain.Entities.Referencia.Rh;

/// <summary>
/// Piso regional por UF (RS, PR, SC, SP). Pode ter múltiplas faixas na mesma vigência —
/// <c>FaixaDescricao</c> identifica a categoria profissional.
/// </summary>
public sealed class SalarioMinimoRegional
{
    public Guid Id { get; set; }
    public string Uf { get; set; } = string.Empty;
    public string CompetenciaInicio { get; set; } = string.Empty;
    public string? CompetenciaFim { get; set; }
    public string FaixaDescricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string SeedOrigem { get; set; } = "upload-admin";
    public DateTime ImportadoEm { get; set; }
    public Guid? ImportadoPor { get; set; }
}
