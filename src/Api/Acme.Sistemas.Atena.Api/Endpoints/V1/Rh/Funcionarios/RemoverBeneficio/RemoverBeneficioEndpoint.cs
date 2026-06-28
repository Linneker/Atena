using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.RemoverBeneficio;

public sealed class RemoverBeneficioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/rh/funcionarios/{id:guid}/beneficios/{vinculoId:guid}", async (
            Guid id,
            Guid vinculoId,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new RemoverBeneficioRequest(id, vinculoId).ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhFuncionario, Permissions.Acoes.Editar))
        .WithTags("RH - Funcionários")
        .WithName("RemoverBeneficio")
        .Produces<RemoverBeneficioResponse>()
        .ProducesProblem(404);
    }
}
