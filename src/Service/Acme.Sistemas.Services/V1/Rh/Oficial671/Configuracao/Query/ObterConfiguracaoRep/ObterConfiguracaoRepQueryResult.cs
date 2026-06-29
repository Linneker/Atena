using Acme.Sistemas.Domain.Enums.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ObterConfiguracaoRep;

public sealed record ObterConfiguracaoRepQueryResult(
    Guid Id,
    Guid EmpresaId,
    TipoRep Tipo,
    string RazaoSocial,
    string CnpjCei,
    string? Cno,
    string? InscricaoEstadual,
    string? CnaePrincipal,
    EnderecoRepDto Endereco,
    Guid CertificadoId,
    string ResponsavelCpf,
    string ResponsavelNome);

public sealed record EnderecoRepDto(
    string Logradouro, string? Numero, string? Complemento,
    string? Bairro, string Cidade, string Uf, string? Cep);
