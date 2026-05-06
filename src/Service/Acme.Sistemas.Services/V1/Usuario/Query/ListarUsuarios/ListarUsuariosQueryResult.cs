using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Usuario.Query.ListarUsuarios;

public sealed record ListarUsuariosQueryItem(
    Guid Id,
    string NomeCompleto,
    string Email,
    StatusAtivo Status,
    DateTime? LastLoginAt,
    DateTime CreatedAt);

public sealed record ListarUsuariosQueryResult(IReadOnlyList<ListarUsuariosQueryItem> Items, int Total);
