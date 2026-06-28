using Acme.Sistemas.Core.Mediators;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FluxoDeCaixa.ObterFluxo;

public sealed class ObterFluxoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/fluxo-de-caixa", async (
            [AsParameters] ObterFluxoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("FluxoDeCaixa")
        .WithName("ObterFluxoDeCaixa")
        .Produces<ObterFluxoResponse>();
    }
}
