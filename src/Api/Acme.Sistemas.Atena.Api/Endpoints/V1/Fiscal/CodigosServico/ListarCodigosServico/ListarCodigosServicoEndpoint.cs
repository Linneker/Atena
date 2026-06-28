using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fiscal.CodigosServico.ListarCodigosServico;

public sealed class ListarCodigosServicoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/fiscal/codigos-servico", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ListarCodigosServicoRequest();
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Fiscal")
        .WithName("ListarCodigosServico")
        .Produces<ListarCodigosServicoResponse>();
    }
}
