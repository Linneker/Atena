using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios.ListarUsuarios;

public sealed record ListarUsuariosResponseItem(
    Guid Id,
    string NomeCompleto,
    string Email,
    StatusAtivo Status,
    DateTime? LastLoginAt,
    DateTime CreatedAt);

public sealed record ListarUsuariosResponse(
    IReadOnlyList<ListarUsuariosResponseItem> Items,
    int Total);
