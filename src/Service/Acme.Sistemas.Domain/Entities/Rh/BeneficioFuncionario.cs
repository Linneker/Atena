namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class BeneficioFuncionario : BaseEntity
{
    public Guid FuncionarioId { get; set; }
    public Guid BeneficioCatalogoId { get; set; }
    public decimal? Valor { get; set; }
    public decimal? DescontoFuncionarioPct { get; set; }
    public DateOnly VigenciaInicio { get; set; }
    public DateOnly? VigenciaFim { get; set; }
    public string? Observacao { get; set; }
}
