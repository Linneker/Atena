using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class HistoricoSalario : BaseEntity
{
    public Guid FuncionarioId { get; set; }
    public decimal Valor { get; set; }
    public DateOnly VigenciaInicio { get; set; }
    public DateOnly? VigenciaFim { get; set; }
    public MotivoSalario Motivo { get; set; }
    public string? Observacao { get; set; }
    public Guid? RegistradoPorUsuarioId { get; set; }
    public DateTime? RegistradoAt { get; set; }
}
