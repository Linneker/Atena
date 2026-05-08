using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas.CriarPlanoDeContas;

public sealed class CriarPlanoDeContasEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/plano-de-contas", async (
            CriarPlanoDeContasRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/plano-de-contas/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("PlanoDeContas")
        .WithName("CriarPlanoDeContas")
        .Produces<CriarPlanoDeContasResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
