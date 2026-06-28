namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class BancoHorasSaldo : BaseEntity
{
    public Guid FuncionarioId { get; set; }
    /// <summary>Formato YYYY-MM.</summary>
    public string Competencia { get; set; } = string.Empty;
    public decimal HorasDevidas { get; set; }
    public decimal HorasRealizadas { get; set; }
    /// <summary>Positivo = funcionário a favor; negativo = devedor.</summary>
    public int SaldoMinutos { get; set; }
    public Guid? PoliticaId { get; set; }
}
