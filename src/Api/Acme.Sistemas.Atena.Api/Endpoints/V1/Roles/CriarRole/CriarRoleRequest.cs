namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles.CriarRole;

public sealed record CriarRoleRequest(
    string Nome,
    string? Descricao,
    IReadOnlyList<string>? PermissoesCodigos);
