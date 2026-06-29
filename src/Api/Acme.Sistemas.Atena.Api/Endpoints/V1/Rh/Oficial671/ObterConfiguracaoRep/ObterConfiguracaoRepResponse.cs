using Acme.Sistemas.Domain.Enums.Rh;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ObterConfiguracaoRep;

public sealed record EnderecoRepOutput(
    string Logradouro, string? Numero, string? Complemento,
    string? Bairro, string Cidade, string Uf, string? Cep);

public sealed record ObterConfiguracaoRepResponse(
    Guid Id,
    Guid EmpresaId,
    TipoRep Tipo,
    string RazaoSocial,
    string CnpjCei,
    string? Cno,
    string? InscricaoEstadual,
    string? CnaePrincipal,
    EnderecoRepOutput Endereco,
    Guid CertificadoId,
    string ResponsavelCpf,
    string ResponsavelNome);
