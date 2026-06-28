using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.RejeitarAjuste;

public sealed class RejeitarAjusteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/ponto/ajustes/{id:guid}/rejeitar", async (
            Guid id,
            RejeitarAjusteRequest body,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = body with { Id = id };
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.AprovarPonto))
        .WithTags("RH - Ponto")
        .WithName("RejeitarAjuste")
        .Produces<RejeitarAjusteResponse>()
        .ProducesProblem(404)
        .ProducesValidationProblem();
    }
}
