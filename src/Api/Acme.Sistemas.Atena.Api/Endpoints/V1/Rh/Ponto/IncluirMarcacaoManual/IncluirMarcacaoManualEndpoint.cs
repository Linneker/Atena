using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.IncluirMarcacaoManual;

public sealed class IncluirMarcacaoManualEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/ponto/manual", async (
            IncluirMarcacaoManualRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Created($"/api/v1/rh/ponto/proprio/{result.Content.Id}", result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.Editar))
        .WithTags("RH - Ponto")
        .WithName("IncluirMarcacaoManual")
        .Produces<IncluirMarcacaoManualResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
