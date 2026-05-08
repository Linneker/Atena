using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Entities.Auditoria;
using Acme.Sistemas.Services.V1.Auditoria.Query.ListarLogs;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.ListarLogs;

public sealed class ListarLogsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/auditoria/logs", async (
            Guid? userId, string? entidade, OperacaoAuditoria? operacao,
            DateTime? inicio, DateTime? fim, int? skip, int? take,
            IMediator m, CancellationToken ct) =>
        {
            var q = new ListarLogsQuery(userId, entidade, operacao, inicio, fim, skip ?? 0, take ?? 50);
            var r = await m.Send(q, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Auditoria, Permissions.Acoes.Ler))
        .WithTags("Auditoria")
        .WithName("ListarLogsAuditoria")
        .Produces<ListarLogsQueryResult>();
    }
}
