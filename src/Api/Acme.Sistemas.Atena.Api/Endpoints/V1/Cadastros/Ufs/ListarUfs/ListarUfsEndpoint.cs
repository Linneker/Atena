using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Cadastros.Ufs.ListarUfs;

public sealed class ListarUfsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/cadastros/ufs", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ListarUfsRequest();
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Cadastros")
        .WithName("ListarUfs")
        .Produces<ListarUfsResponse>();
    }
}
