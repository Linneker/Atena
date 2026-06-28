using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.VincularBeneficio;

public sealed class VincularBeneficioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/funcionarios/{id:guid}/beneficios", async (
            Guid id,
            VincularBeneficioRequest body,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = body with { FuncionarioId = id };
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created(
                $"/api/v1/rh/funcionarios/{id}/beneficios/{response.Id}", response);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhFuncionario, Permissions.Acoes.Editar))
        .WithTags("RH - Funcionários")
        .WithName("VincularBeneficio")
        .Produces<VincularBeneficioResponse>(StatusCodes.Status201Created)
        .ProducesProblem(404)
        .ProducesValidationProblem();
    }
}
