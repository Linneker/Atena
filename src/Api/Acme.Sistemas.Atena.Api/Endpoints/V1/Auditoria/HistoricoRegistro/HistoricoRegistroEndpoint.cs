using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.HistoricoRegistro;

public sealed class HistoricoRegistroEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/auditoria/historico/{entidade}/{id:guid}", async (
            string entidade,
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new HistoricoRegistroRequest(entidade, id);
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Auditoria, Permissions.Acoes.Ler))
        .WithTags("Auditoria")
        .WithName("HistoricoRegistroAuditoria")
        .Produces<HistoricoRegistroResponse>();
    }
}
