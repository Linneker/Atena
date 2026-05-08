using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Roles.Command.CriarRole;

public sealed record CriarRoleCommandResult(Guid Id, string Nome);
