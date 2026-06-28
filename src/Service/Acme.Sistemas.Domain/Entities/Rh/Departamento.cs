namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class Departamento : BaseEntity
{
    public string? Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public Guid? CentroDeCustoId { get; set; }
    public bool Ativo { get; set; } = true;
}
