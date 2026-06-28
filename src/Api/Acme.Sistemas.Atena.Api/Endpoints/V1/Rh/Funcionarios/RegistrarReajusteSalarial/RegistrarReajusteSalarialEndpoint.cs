using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.RegistrarReajusteSalarial;

public sealed class RegistrarReajusteSalarialEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/funcionarios/{id:guid}/salarios", async (
            Guid id,
            RegistrarReajusteSalarialRequest body,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = body with { FuncionarioId = id };
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created(
                $"/api/v1/rh/funcionarios/{id}/salarios/{response.HistoricoSalarioId}",
                response);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhFuncionario, Permissions.Acoes.Editar))
        .WithTags("RH - Funcionários")
        .WithName("RegistrarReajusteSalarial")
        .Produces<RegistrarReajusteSalarialResponse>(StatusCodes.Status201Created)
        .ProducesProblem(404)
        .ProducesValidationProblem();
    }
}
