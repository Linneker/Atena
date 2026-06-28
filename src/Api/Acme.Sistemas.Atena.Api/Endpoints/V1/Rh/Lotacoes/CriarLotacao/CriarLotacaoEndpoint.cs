using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Lotacoes.CriarLotacao;

public sealed class CriarLotacaoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/rh/lotacoes", async (
            CriarLotacaoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/rh/lotacoes/{response.Id}", response);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhLotacao, Permissions.Acoes.Criar))
        .WithTags("RH - Lotações")
        .WithName("CriarLotacao")
        .Produces<CriarLotacaoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
