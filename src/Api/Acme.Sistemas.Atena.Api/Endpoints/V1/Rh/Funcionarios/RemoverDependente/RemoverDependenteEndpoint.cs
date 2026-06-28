using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.RemoverDependente;

public sealed class RemoverDependenteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/rh/funcionarios/{id:guid}/dependentes/{depId:guid}", async (
            Guid id,
            Guid depId,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new RemoverDependenteRequest(id, depId).ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhDependente, Permissions.Acoes.Excluir))
        .WithTags("RH - Funcionários")
        .WithName("RemoverDependente")
        .Produces<RemoverDependenteResponse>()
        .ProducesProblem(404);
    }
}
