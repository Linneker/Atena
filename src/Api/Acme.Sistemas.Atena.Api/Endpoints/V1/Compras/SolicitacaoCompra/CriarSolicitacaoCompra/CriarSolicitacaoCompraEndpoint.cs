using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.CriarSolicitacaoCompra;

public sealed class CriarSolicitacaoCompraEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/solicitacoes-compra", async (
            CriarSolicitacaoCompraRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/solicitacoes-compra/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("SolicitacoesCompra")
        .WithName("CriarSolicitacaoCompra")
        .Produces<CriarSolicitacaoCompraResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
