using Acme.Sistemas.Domain.Enums.Rh;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.SalvarConfiguracaoRep;

public sealed record SalvarConfiguracaoRepRequest(
    Guid EmpresaId,
    TipoRep Tipo,
    string RazaoSocial,
    string CnpjCei,
    string? Cno,
    string? InscricaoEstadual,
    string? CnaePrincipal,
    EnderecoRepInput Endereco,
    Guid CertificadoId,
    string ResponsavelCpf,
    string ResponsavelNome);

public sealed record EnderecoRepInput(
    string Logradouro, string? Numero, string? Complemento,
    string? Bairro, string Cidade, string Uf, string? Cep);
