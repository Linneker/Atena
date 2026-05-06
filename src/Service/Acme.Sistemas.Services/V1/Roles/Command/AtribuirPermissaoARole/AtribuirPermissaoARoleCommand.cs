using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Roles.Command.AtribuirPermissaoARole;

public sealed record AtribuirPermissaoARoleCommand(Guid RoleId, string PermissaoCodigo)
    : IRequest<ResponseDefault>;
