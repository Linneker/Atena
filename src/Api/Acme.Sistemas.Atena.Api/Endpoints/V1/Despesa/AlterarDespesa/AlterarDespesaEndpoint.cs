using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Despesa.Command.AlterarDespesa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.AlterarDespesa;
public sealed record AlterarDespesaRequest(
    string Nome,
    string? Descricao,
    string? Categoria,
    decimal Valor,
    bool DespesaFixa,
    DateTime DataVencimento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    Guid? FornecedorId);

public sealed class AlterarDespesaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/despesas/{id:guid}", async (
            Guid id,
            AlterarDespesaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new AlterarDespesaCommand(
                id, request.Nome, request.Descricao, request.Categoria,
                request.Valor, request.DespesaFixa, request.DataVencimento,
                request.CompetenciaId, request.CentroDeCustoId, request.FornecedorId);

            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("AlterarDespesa")
        .Produces<AlterarDespesaCommandResult>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
