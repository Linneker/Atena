using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Funcionarios.CriarFuncionario;

public sealed class CriarFuncionarioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/funcionarios", async (
            CriarFuncionarioRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/funcionarios/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("Funcionarios")
        .WithName("CriarFuncionario")
        .Produces<CriarFuncionarioResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
