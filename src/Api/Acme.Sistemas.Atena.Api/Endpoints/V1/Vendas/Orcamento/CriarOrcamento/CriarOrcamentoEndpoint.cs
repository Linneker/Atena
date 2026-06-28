using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Orcamento.CriarOrcamento;

public sealed class CriarOrcamentoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/orcamentos", async (
            CriarOrcamentoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/orcamentos/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("Orcamentos")
        .WithName("CriarOrcamento")
        .Produces<CriarOrcamentoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
