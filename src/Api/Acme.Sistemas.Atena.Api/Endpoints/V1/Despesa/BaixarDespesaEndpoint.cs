using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Despesa.Command.BaixarDespesa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa;

public sealed record BaixarDespesaRequest(
    decimal ValorPago,
    DateTime DataPagamento,
    FormaPagamento FormaPagamento,
    string? Observacao);

public sealed class BaixarDespesaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/despesas/{id:guid}/baixar", async (
            Guid id,
            BaixarDespesaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new BaixarDespesaCommand(
                id, request.ValorPago, request.DataPagamento,
                request.FormaPagamento, request.Observacao);

            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("BaixarDespesa")
        .Produces<BaixarDespesaCommandResult>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
