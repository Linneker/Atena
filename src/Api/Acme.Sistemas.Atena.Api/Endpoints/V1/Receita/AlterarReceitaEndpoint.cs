using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Receita.Command.AlterarReceita;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita;

public sealed record AlterarReceitaRequest(
    string Nome,
    string? Descricao,
    string? Categoria,
    decimal Valor,
    bool ReceitaFixa,
    DateTime DataPrevistaRecebimento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    Guid? ClienteId,
    Guid? OrigemVendaId);

public sealed class AlterarReceitaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/receitas/{id:guid}", async (
            Guid id,
            AlterarReceitaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new AlterarReceitaCommand(
                id, request.Nome, request.Descricao, request.Categoria,
                request.Valor, request.ReceitaFixa, request.DataPrevistaRecebimento,
                request.CompetenciaId, request.CentroDeCustoId,
                request.ClienteId, request.OrigemVendaId);

            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Receitas")
        .WithName("AlterarReceita")
        .Produces<AlterarReceitaCommandResult>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
