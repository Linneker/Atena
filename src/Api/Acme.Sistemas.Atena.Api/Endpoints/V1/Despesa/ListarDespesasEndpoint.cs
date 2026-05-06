using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Despesa.Query.ListarDespesas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa;

public sealed class ListarDespesasEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/despesas", async (
            StatusPagamento? status,
            DateTime? vencimentoInicio,
            DateTime? vencimentoFim,
            string? categoria,
            Guid? competenciaId,
            int? skip,
            int? take,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new ListarDespesasQuery(
                status, vencimentoInicio, vencimentoFim, categoria,
                competenciaId, skip ?? 0, take ?? 50);

            var response = await mediator.Send(query, cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("ListarDespesas")
        .Produces<ListarDespesasQueryResult>();
    }
}
