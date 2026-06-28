using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Rh;

public sealed class Dependente : BaseEntity
{
    public Guid FuncionarioId { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public DateOnly DataNascimento { get; set; }
    public TipoDependente Tipo { get; set; }
    public bool Irrf { get; set; }
    public bool SalarioFamilia { get; set; }
    public decimal? PensaoAlimenticiaPct { get; set; }
    public DateOnly? DataInicio { get; set; }
    public DateOnly? DataFim { get; set; }
}
