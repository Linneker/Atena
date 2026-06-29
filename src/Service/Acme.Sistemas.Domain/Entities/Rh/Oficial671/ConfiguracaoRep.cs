using Acme.Sistemas.Domain.Enums.Rh;

namespace Acme.Sistemas.Domain.Entities.Rh.Oficial671;

/// <summary>
/// Configuração do REP por empresa do tenant — Portaria 671/2021.
/// Sem essa config completa, a empresa não pode ativar <c>usa_rep_oficial</c>.
/// </summary>
public sealed class ConfiguracaoRep : BaseEntity
{
    public Guid EmpresaId { get; set; }
    public TipoRep Tipo { get; set; } = TipoRep.RepC;

    public string RazaoSocial { get; set; } = string.Empty;
    public string CnpjCei { get; set; } = string.Empty;
    public string? Cno { get; set; }
    public string? InscricaoEstadual { get; set; }
    public string? CnaePrincipal { get; set; }

    public string EnderecoLogradouro { get; set; } = string.Empty;
    public string? EnderecoNumero { get; set; }
    public string? EnderecoComplemento { get; set; }
    public string? EnderecoBairro { get; set; }
    public string EnderecoCidade { get; set; } = string.Empty;
    public string EnderecoUf { get; set; } = string.Empty;
    public string? EnderecoCep { get; set; }

    public Guid CertificadoId { get; set; }
    public string ResponsavelCpf { get; set; } = string.Empty;
    public string ResponsavelNome { get; set; } = string.Empty;
}
