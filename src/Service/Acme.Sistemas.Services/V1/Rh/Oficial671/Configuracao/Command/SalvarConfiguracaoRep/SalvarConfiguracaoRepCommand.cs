using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Command.SalvarConfiguracaoRep;

public sealed record SalvarConfiguracaoRepCommand(
    Guid EmpresaId,
    TipoRep Tipo,
    string RazaoSocial,
    string CnpjCei,
    string? Cno,
    string? InscricaoEstadual,
    string? CnaePrincipal,
    string EnderecoLogradouro,
    string? EnderecoNumero,
    string? EnderecoComplemento,
    string? EnderecoBairro,
    string EnderecoCidade,
    string EnderecoUf,
    string? EnderecoCep,
    Guid CertificadoId,
    string ResponsavelCpf,
    string ResponsavelNome) : IRequest<ResponseDefault<SalvarConfiguracaoRepCommandResult>>;
