using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Jornadas.AlterarJornada;

public sealed class AlterarJornadaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/rh/jornadas/{id:guid}", async (
            Guid id,
            AlterarJornadaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhJornada, Permissions.Acoes.Editar))
        .WithTags("RH - Jornadas")
        .WithName("AlterarJornada")
        .Produces<AlterarJornadaResponse>()
        .ProducesProblem(404)
        .ProducesValidationProblem();
    }
}
