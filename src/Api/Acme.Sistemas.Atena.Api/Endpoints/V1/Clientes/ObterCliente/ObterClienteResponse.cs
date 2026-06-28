using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.ObterCliente;

public sealed record ObterClienteResponse(
    Guid Id,
    TipoPessoa Tipo,
    string Nome,
    string? NomeFantasia,
    string Documento,
    string? InscricaoEstadual,
    string? Email,
    string? Telefone,
    StatusAtivo Status,
    bool Inadimplente,
    bool BloqueadoVendas,
    Endereco Endereco,
    DateTime CreatedAt);
