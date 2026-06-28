using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Admin.ListarDispositivosMobile;

public sealed class ListarDispositivosMobileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/admin/mobile/dispositivos", async (
            int? skip, int? take, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(
                new ListarDispositivosMobileRequest(skip ?? 0, take ?? 50).ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Admin, Permissions.Acoes.SeedTenant))
        .WithTags("Admin")
        .WithName("ListarDispositivosMobile")
        .Produces<ListarDispositivosMobileResponse>();
    }
}
