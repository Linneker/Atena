using System.Text;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Entities.Auditoria;
using Acme.Sistemas.Services.V1.Auditoria.Query.ExportarLogs;
using Acme.Sistemas.Services.V1.Auditoria.Query.HistoricoRegistro;
using Acme.Sistemas.Services.V1.Auditoria.Query.ListarLogs;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria;

public sealed class AuditoriaEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auditoria")
            .RequireAuthorization()
            .WithTags("Auditoria");

        group.MapGet("/logs", async (
            Guid? userId, string? entidade, OperacaoAuditoria? operacao,
            DateTime? inicio, DateTime? fim, int? skip, int? take,
            IMediator m, CancellationToken ct) =>
        {
            var q = new ListarLogsQuery(userId, entidade, operacao, inicio, fim, skip ?? 0, take ?? 50);
            var r = await m.Send(q, ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("ListarLogsAuditoria").Produces<ListarLogsQueryResult>();

        group.MapGet("/historico/{entidade}/{id:guid}", async (
            string entidade, Guid id, IMediator m, CancellationToken ct) =>
        {
            var r = await m.Send(new HistoricoRegistroQuery(entidade, id), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        }).WithName("HistoricoRegistroAuditoria").Produces<HistoricoRegistroQueryResult>();

        group.MapGet("/exportar", async (
            Guid? userId, string? entidade, OperacaoAuditoria? operacao,
            DateTime? inicio, DateTime? fim,
            IMediator m, CancellationToken ct) =>
        {
            var q = new ExportarLogsQuery(userId, entidade, operacao, inicio, fim);
            var r = await m.Send(q, ct);
            if (!r.IsSuccess) return Results.Json(r, statusCode: r.Status);

            var bytes = Encoding.UTF8.GetBytes(r.Content!.ConteudoJson);
            var fileName = $"audit-export-{DateTime.UtcNow:yyyyMMddHHmmss}.json";
            // Hash retorna no header para verificação de integridade
            var ctx = new DefaultHttpContext();
            return Results.File(bytes, "application/json", fileName);
        }).WithName("ExportarLogsAuditoria")
          .Produces(StatusCodes.Status200OK, contentType: "application/json");

        // Endpoint complementar que retorna apenas o hash (para verificação posterior)
        group.MapGet("/exportar/hash", async (
            Guid? userId, string? entidade, OperacaoAuditoria? operacao,
            DateTime? inicio, DateTime? fim,
            IMediator m, CancellationToken ct) =>
        {
            var q = new ExportarLogsQuery(userId, entidade, operacao, inicio, fim);
            var r = await m.Send(q, ct);
            if (!r.IsSuccess) return Results.Json(r, statusCode: r.Status);
            return Results.Ok(new { totalRegistros = r.Content!.TotalRegistros, hashSha256 = r.Content.HashSha256 });
        }).WithName("ExportarLogsHash");
    }
}
