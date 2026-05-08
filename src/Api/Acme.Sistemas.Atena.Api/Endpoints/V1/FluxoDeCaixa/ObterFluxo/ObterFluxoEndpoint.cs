using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.FluxoDeCaixa.Query.ObterFluxo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FluxoDeCaixa.ObterFluxo;
public sealed class ObterFluxoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/fluxo-de-caixa", async (
            DateTime inicio,
            DateTime fim,
            bool? somenteRealizados,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(
                new ObterFluxoQuery(inicio, fim, somenteRealizados ?? false),
                cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("FluxoDeCaixa")
        .WithName("ObterFluxoDeCaixa")
        .Produces<ObterFluxoQueryResult>();
    }
}
