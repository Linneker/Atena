using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

public sealed record CriarEmpresaCommand(
    string RazaoSocial,
    string? NomeFantasia,
    string Cnpj,
    string? InscricaoEstadual,
    string? InscricaoMunicipal,
    string? Email,
    string? Telefone,
    EnderecoDto? Endereco,
    bool BuscarEnderecoPorCep = false) : IRequest<ResponseDefault<CriarEmpresaCommandResult>>;

public sealed record CriarEmpresaCommandResult(Guid Id, string RazaoSocial, string Cnpj);
