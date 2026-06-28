using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios.ObterUsuario;

public sealed record ObterUsuarioResponse(
    Guid Id,
    string NomeCompleto,
    string Email,
    StatusAtivo Status,
    int FailedLoginAttempts,
    DateTime? LockedUntil,
    DateTime? LastLoginAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
