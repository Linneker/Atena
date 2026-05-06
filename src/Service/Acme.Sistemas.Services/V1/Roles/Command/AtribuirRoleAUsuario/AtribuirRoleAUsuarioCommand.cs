using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Roles.Command.AtribuirRoleAUsuario;

public sealed record AtribuirRoleAUsuarioCommand(Guid UserId, Guid RoleId, DateTime? ExpiresAt)
    : IRequest<ResponseDefault>;
