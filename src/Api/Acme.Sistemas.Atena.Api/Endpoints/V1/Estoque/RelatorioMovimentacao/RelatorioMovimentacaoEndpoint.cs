using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RelatorioMovimentacao;

public sealed class RelatorioMovimentacaoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/estoque/produtos/{produtoId:guid}/movimentacao", async (
            Guid produtoId,
            DateTime? inicio,
            DateTime? fim,
            int? skip,
            int? take,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new RelatorioMovimentacaoRequest(produtoId, inicio, fim, skip ?? 0, take ?? 200);
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Estoque")
        .WithName("RelatorioMovimentacao")
        .Produces<RelatorioMovimentacaoResponse>();
    }
}
