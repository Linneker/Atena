using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Usuario.Query.ObterUsuario;

public sealed record ObterUsuarioQueryResult(
    Guid Id,
    string NomeCompleto,
    string Email,
    StatusAtivo Status,
    int FailedLoginAttempts,
    DateTime? LockedUntil,
    DateTime? LastLoginAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
