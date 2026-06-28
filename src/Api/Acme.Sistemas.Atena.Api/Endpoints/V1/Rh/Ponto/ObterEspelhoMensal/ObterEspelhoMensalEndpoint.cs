using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ObterEspelhoMensal;

public sealed class ObterEspelhoMensalEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/ponto/espelho", async (
            Guid funcionarioId,
            string competencia,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(
                new ObterEspelhoMensalRequest(funcionarioId, competencia).ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.Ler))
        .WithTags("RH - Ponto")
        .WithName("ObterEspelhoMensal")
        .Produces<ObterEspelhoMensalResponse>()
        .ProducesProblem(404)
        .ProducesValidationProblem();
    }
}
