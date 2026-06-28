using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.ObterFichaCompleta;

public sealed class ObterFichaCompletaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/funcionarios/{id:guid}/ficha", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new ObterFichaCompletaRequest(id).ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhFuncionario, Permissions.Acoes.Ler))
        .WithTags("RH - Funcionários")
        .WithName("ObterFichaCompleta")
        .Produces<ObterFichaCompletaResponse>()
        .ProducesProblem(404);
    }
}
