using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Entities.Cadastros;

public sealed class Funcionario : BaseEntity
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefone { get; set; }

    // OBSOLETOS desde rh-fundacao (W1). Use CargoId / DepartamentoId. Remoção planejada para W3.
    public string? Cargo { get; set; }
    public string? Departamento { get; set; }

    public Guid? CentroDeCustoId { get; set; }
    public DateTime? DataAdmissao { get; set; }
    public DateTime? DataDemissao { get; set; }
    public Guid? UsuarioId { get; set; }
    public StatusAtivo Status { get; set; } = StatusAtivo.Ativo;

    public Guid? CargoId { get; set; }
    public Guid? LotacaoId { get; set; }
    public Guid? DepartamentoId { get; set; }
    public TipoContrato? TipoContrato { get; set; }
    public RegimeRemuneracao? RegimeRemuneracao { get; set; }
    public string? CodigoMatricula { get; set; }
    public string? Pis { get; set; }
    public string? Ctps { get; set; }
    public string? CtpsSerie { get; set; }
    public string? CtpsUf { get; set; }
    public string? Rg { get; set; }
    public string? RgOrgao { get; set; }
    public string? RgUf { get; set; }
    public EstadoCivil? EstadoCivil { get; set; }
    public string? Naturalidade { get; set; }
    public string? Nacionalidade { get; set; } = "Brasileira";

    public string? EnderecoJson { get; set; }
    public string? ContaBancariaJson { get; set; }

    public EnderecoFuncionario? Endereco { get; set; }
    public ContaBancariaFuncionario? ContaBancaria { get; set; }
}
