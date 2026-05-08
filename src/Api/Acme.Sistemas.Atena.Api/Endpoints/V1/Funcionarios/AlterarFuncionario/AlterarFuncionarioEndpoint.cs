using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Funcionarios.AlterarFuncionario;

public sealed class AlterarFuncionarioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/funcionarios/{id:guid}", async (
            Guid id,
            AlterarFuncionarioRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Funcionarios")
        .WithName("AlterarFuncionario")
        .Produces<AlterarFuncionarioResponse>()
        .ProducesValidationProblem();
    }
}
