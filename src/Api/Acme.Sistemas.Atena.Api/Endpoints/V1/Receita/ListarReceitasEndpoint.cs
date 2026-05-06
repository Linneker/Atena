using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Receita.Query.ListarReceitas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita;

public sealed class ListarReceitasEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/receitas", async (
            StatusPagamento? status,
            DateTime? recebimentoInicio,
            DateTime? recebimentoFim,
            string? categoria,
            Guid? competenciaId,
            int? skip,
            int? take,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new ListarReceitasQuery(
                status, recebimentoInicio, recebimentoFim, categoria,
                competenciaId, skip ?? 0, take ?? 50);

            var response = await mediator.Send(query, cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Receitas")
        .WithName("ListarReceitas")
        .Produces<ListarReceitasQueryResult>();
    }
}
