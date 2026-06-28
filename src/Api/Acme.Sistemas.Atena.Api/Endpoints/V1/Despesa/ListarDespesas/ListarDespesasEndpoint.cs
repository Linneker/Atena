using Acme.Sistemas.Core.Mediators;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.ListarDespesas;

public sealed class ListarDespesasEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/despesas", async (
            [AsParameters] ListarDespesasRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("ListarDespesas")
        .Produces<ListarDespesasResponse>();
    }
}
