using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class MovimentoBancoHoras : BaseEntity
{
    public Guid FuncionarioId { get; set; }
    public DateOnly Data { get; set; }
    public OrigemMovimentoBancoHoras Origem { get; set; }
    /// <summary>Positivo ou negativo.</summary>
    public int Minutos { get; set; }
    public Guid? ReferenciaMarcacaoId { get; set; }
    public string Competencia { get; set; } = string.Empty;
    public string? Observacao { get; set; }
}
