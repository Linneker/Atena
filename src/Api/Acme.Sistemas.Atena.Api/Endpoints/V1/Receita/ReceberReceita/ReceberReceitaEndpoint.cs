using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Receita.Command.ReceberReceita;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ReceberReceita;
public sealed record ReceberReceitaRequest(
    decimal ValorRecebido,
    DateTime DataRecebimento,
    FormaPagamento FormaPagamento,
    string? Observacao);

public sealed class ReceberReceitaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/receitas/{id:guid}/receber", async (
            Guid id,
            ReceberReceitaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new ReceberReceitaCommand(
                id, request.ValorRecebido, request.DataRecebimento,
                request.FormaPagamento, request.Observacao);

            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Receitas")
        .WithName("ReceberReceita")
        .Produces<ReceberReceitaCommandResult>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
