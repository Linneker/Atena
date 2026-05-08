using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Aging.AgingContasReceber;

public sealed class AgingContasReceberEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/relatorios/contas-receber/aging", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new AgingContasReceberRequest();
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Relatorios")
        .WithName("AgingContasReceber")
        .Produces<AgingContasReceberResponse>();
    }
}
