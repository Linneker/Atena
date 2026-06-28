using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cargos.CriarCargo;

public sealed class CriarCargoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/cargos", async (
            CriarCargoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/rh/cargos/{response.Id}", response);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhCargo, Permissions.Acoes.Criar))
        .WithTags("RH - Cargos")
        .WithName("CriarCargo")
        .Produces<CriarCargoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
