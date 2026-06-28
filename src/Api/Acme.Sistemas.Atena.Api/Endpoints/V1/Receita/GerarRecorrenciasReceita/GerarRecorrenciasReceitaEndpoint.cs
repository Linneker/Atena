using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.GerarRecorrenciasReceita;

public sealed class GerarRecorrenciasReceitaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/receitas/gerar-recorrencias", async (
            GerarRecorrenciasReceitaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Receitas")
        .WithName("GerarRecorrenciasReceita")
        .Produces<GerarRecorrenciasReceitaResponse>();
    }
}
