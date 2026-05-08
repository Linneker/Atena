namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles.ListarRoles;

public sealed record ListarRolesResponseItem(Guid Id, string Nome, string? Descricao, bool IsSystem);

public sealed record ListarRolesResponse(IReadOnlyList<ListarRolesResponseItem> Items, int Total);
