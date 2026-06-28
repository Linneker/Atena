using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.SolicitarAjuste;

public sealed class SolicitarAjusteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/ponto/ajustes", async (
            SolicitarAjusteRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Created($"/api/v1/rh/ponto/ajustes/{result.Content.Id}", result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.AjustarPonto))
        .WithTags("RH - Ponto")
        .WithName("SolicitarAjuste")
        .Produces<SolicitarAjusteResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
