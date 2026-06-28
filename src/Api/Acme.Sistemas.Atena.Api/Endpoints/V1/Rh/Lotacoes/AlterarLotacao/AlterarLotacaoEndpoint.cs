using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Lotacoes.AlterarLotacao;

public sealed class AlterarLotacaoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/rh/lotacoes/{id:guid}", async (
            Guid id,
            AlterarLotacaoRequest body,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = body with { Id = id };
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhLotacao, Permissions.Acoes.Editar))
        .WithTags("RH - Lotações")
        .WithName("AlterarLotacao")
        .Produces<AlterarLotacaoResponse>()
        .ProducesProblem(404)
        .ProducesValidationProblem();
    }
}
