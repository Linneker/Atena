namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ConciliacaoBancaria.ImportarExtrato;

// Request reflete o multipart/form-data:
//  banco (string), agencia (string?), conta (string?), formato (string?, default CSV),
//  arquivo (IFormFile — extraído pelo endpoint via ReadFormAsync).
public sealed record ImportarExtratoRequest(
    string Banco,
    string? Agencia,
    string? Conta,
    string Formato,
    byte[] Arquivo);
