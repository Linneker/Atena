using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Departamentos.AlterarDepartamento;

public sealed class AlterarDepartamentoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/rh/departamentos/{id:guid}", async (
            Guid id,
            AlterarDepartamentoRequest body,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = body with { Id = id };
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhDepartamento, Permissions.Acoes.Editar))
        .WithTags("RH - Departamentos")
        .WithName("AlterarDepartamento")
        .Produces<AlterarDepartamentoResponse>()
        .ProducesProblem(404)
        .ProducesValidationProblem();
    }
}
