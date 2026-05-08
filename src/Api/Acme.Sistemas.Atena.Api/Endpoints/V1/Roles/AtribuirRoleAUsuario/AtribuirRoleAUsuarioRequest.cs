namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles.AtribuirRoleAUsuario;

public sealed record AtribuirRoleAUsuarioRequest(Guid UserId, DateTime? ExpiresAt);
