using System.Text;
using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Entities.Auditoria;
using Acme.Sistemas.Services.V1.Auditoria.Query.ExportarLogs;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.ExportarLogs;

public sealed class ExportarLogsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/auditoria/exportar", async (
            Guid? userId, string? entidade, OperacaoAuditoria? operacao,
            DateTime? inicio, DateTime? fim,
            IMediator m, CancellationToken ct) =>
        {
            var q = new ExportarLogsQuery(userId, entidade, operacao, inicio, fim);
            var r = await m.Send(q, ct);
            if (!r.IsSuccess) return Results.Json(r, statusCode: r.Status);

            var bytes = Encoding.UTF8.GetBytes(r.Content!.ConteudoJson);
            var fileName = $"audit-export-{DateTime.UtcNow:yyyyMMddHHmmss}.json";
            return Results.File(bytes, "application/json", fileName);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Auditoria, Permissions.Acoes.Exportar))
        .WithTags("Auditoria")
        .WithName("ExportarLogsAuditoria")
        .Produces(StatusCodes.Status200OK, contentType: "application/json");

        // Endpoint complementar (mesma pasta) — só hash + total para verificação posterior.
        app.MapGet("/api/v1/auditoria/exportar/hash", async (
            Guid? userId, string? entidade, OperacaoAuditoria? operacao,
            DateTime? inicio, DateTime? fim,
            IMediator m, CancellationToken ct) =>
        {
            var q = new ExportarLogsQuery(userId, entidade, operacao, inicio, fim);
            var r = await m.Send(q, ct);
            if (!r.IsSuccess) return Results.Json(r, statusCode: r.Status);
            return Results.Ok(new { totalRegistros = r.Content!.TotalRegistros, hashSha256 = r.Content.HashSha256 });
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Auditoria, Permissions.Acoes.Exportar))
        .WithTags("Auditoria")
        .WithName("ExportarLogsHash");
    }
}
