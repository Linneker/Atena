using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.CriarCliente;

public sealed record CriarClienteRequest(
    TipoPessoa Tipo,
    string Nome,
    string? NomeFantasia,
    string Documento,
    string? InscricaoEstadual,
    string? Email,
    string? Telefone,
    EnderecoDto? Endereco,
    bool BuscarEnderecoPorCep = false);
