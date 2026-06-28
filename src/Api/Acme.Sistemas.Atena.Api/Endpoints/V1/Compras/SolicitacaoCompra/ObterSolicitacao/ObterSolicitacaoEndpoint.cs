using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.ObterSolicitacao;

public sealed class ObterSolicitacaoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/solicitacoes-compra/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ObterSolicitacaoRequest(id);
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("SolicitacoesCompra")
        .WithName("ObterSolicitacao")
        .Produces<ObterSolicitacaoResponse>()
        .ProducesProblem(404);
    }
}
