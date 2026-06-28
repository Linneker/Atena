namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class EscalaFuncionario : BaseEntity
{
    public Guid FuncionarioId { get; set; }
    public Guid JornadaId { get; set; }
    public DateOnly VigenciaInicio { get; set; }
    public DateOnly? VigenciaFim { get; set; }
    public string? Observacao { get; set; }
}
