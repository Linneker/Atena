using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.RejeitarSolicitacao;

public sealed class RejeitarSolicitacaoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/solicitacoes-compra/{id:guid}/rejeitar", async (
            Guid id,
            RejeitarSolicitacaoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("SolicitacoesCompra")
        .WithName("RejeitarSolicitacao")
        .Produces<RejeitarSolicitacaoResponse>()
        .ProducesValidationProblem();
    }
}
