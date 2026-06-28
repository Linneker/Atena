using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Mobile.DesregistrarDispositivo;

public sealed class DesregistrarDispositivoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/mobile/dispositivos/{deviceId}/desregistrar", async (
            string deviceId,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(
                new DesregistrarDispositivoRequest(deviceId).ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Mobile")
        .WithName("DesregistrarDispositivo")
        .Produces<DesregistrarDispositivoResponse>()
        .ProducesProblem(404);
    }
}
