using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Auditoria.Query.HistoricoRegistro;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.HistoricoRegistro;

public sealed class HistoricoRegistroEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/auditoria/historico/{entidade}/{id:guid}", async (
            string entidade, Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new HistoricoRegistroQuery(entidade, id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Auditoria, Permissions.Acoes.Ler))
        .WithTags("Auditoria")
        .WithName("HistoricoRegistroAuditoria")
        .Produces<HistoricoRegistroQueryResult>();
    }
}
