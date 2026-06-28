using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Admin.RevogarDispositivoMobile;

public sealed class RevogarDispositivoMobileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/admin/mobile/dispositivos/{id:guid}/revogar", async (
            Guid id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(
                new RevogarDispositivoMobileRequest(id).ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Admin, Permissions.Acoes.SeedTenant))
        .WithTags("Admin")
        .WithName("RevogarDispositivoMobile")
        .Produces<RevogarDispositivoMobileResponse>()
        .ProducesProblem(404);
    }
}
