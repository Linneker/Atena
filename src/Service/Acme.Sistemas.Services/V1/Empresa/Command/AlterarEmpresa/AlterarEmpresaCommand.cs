using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Services.V1.Empresa.Command.AlterarEmpresa;

public sealed record AlterarEmpresaCommand(
    Guid Id,
    string RazaoSocial,
    string? NomeFantasia,
    string Cnpj,
    string? InscricaoEstadual,
    string? InscricaoMunicipal,
    string? Email,
    string? Telefone,
    StatusAtivo Status,
    EnderecoDto? Endereco,
    bool BuscarEnderecoPorCep = false) : IRequest<ResponseDefault<AlterarEmpresaCommandResult>>;

public sealed record AlterarEmpresaCommandResult(Guid Id);
