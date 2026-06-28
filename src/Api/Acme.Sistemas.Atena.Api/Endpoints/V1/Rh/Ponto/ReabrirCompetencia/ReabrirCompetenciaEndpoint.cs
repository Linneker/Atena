using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ReabrirCompetencia;

public sealed class ReabrirCompetenciaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/ponto/competencia/reabrir", async (
            ReabrirCompetenciaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.ReabrirCompetencia))
        .WithTags("RH - Ponto")
        .WithName("ReabrirCompetencia")
        .Produces<ReabrirCompetenciaResponse>()
        .ProducesProblem(404)
        .ProducesValidationProblem();
    }
}
