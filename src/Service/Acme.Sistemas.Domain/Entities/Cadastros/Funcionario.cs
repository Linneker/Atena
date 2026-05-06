using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Cadastros;

public sealed class Funcionario : BaseEntity
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Cargo { get; set; }
    public string? Departamento { get; set; }
    public Guid? CentroDeCustoId { get; set; }
    public DateTime? DataAdmissao { get; set; }
    public DateTime? DataDemissao { get; set; }
    public Guid? UsuarioId { get; set; }
    public StatusAtivo Status { get; set; } = StatusAtivo.Ativo;
}
