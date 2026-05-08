using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas.AlterarPlanoDeContas;

public sealed class AlterarPlanoDeContasEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/plano-de-contas/{id:guid}", async (
            Guid id,
            AlterarPlanoDeContasRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("PlanoDeContas")
        .WithName("AlterarPlanoDeContas")
        .Produces<AlterarPlanoDeContasResponse>()
        .ProducesValidationProblem();
    }
}
