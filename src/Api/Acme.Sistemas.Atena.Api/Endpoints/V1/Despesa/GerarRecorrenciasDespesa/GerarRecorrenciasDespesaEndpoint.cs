using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.GerarRecorrenciasDespesa;

public sealed class GerarRecorrenciasDespesaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/despesas/gerar-recorrencias", async (
            GerarRecorrenciasDespesaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("GerarRecorrenciasDespesa")
        .Produces<GerarRecorrenciasDespesaResponse>();
    }
}
