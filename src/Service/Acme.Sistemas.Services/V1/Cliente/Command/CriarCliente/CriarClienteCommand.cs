using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Services.V1.Cliente.Command.CriarCliente;

public sealed record CriarClienteCommand(
    TipoPessoa Tipo,
    string Nome,
    string? NomeFantasia,
    string Documento,
    string? InscricaoEstadual,
    string? Email,
    string? Telefone,
    EnderecoDto? Endereco,
    bool BuscarEnderecoPorCep = false) : IRequest<ResponseDefault<CriarClienteCommandResult>>;

public sealed record CriarClienteCommandResult(Guid Id, string Nome, string Documento);
