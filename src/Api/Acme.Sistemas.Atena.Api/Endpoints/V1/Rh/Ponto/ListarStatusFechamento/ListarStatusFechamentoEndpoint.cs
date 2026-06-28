using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarStatusFechamento;

public sealed class ListarStatusFechamentoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/rh/ponto/competencia/{competencia}/status", async (
            string competencia,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(
                new ListarStatusFechamentoRequest(competencia).ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);
            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.RhPonto, Permissions.Acoes.Ler))
        .WithTags("RH - Ponto")
        .WithName("ListarStatusFechamento")
        .Produces<ListarStatusFechamentoResponse>();
    }
}
