using System.Text;
using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.ExportarLogs;

public sealed class ExportarLogsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/auditoria/exportar", async (
            [AsParameters] ExportarLogsRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var bytes = Encoding.UTF8.GetBytes(result.Content.ConteudoJson);
            var fileName = $"audit-export-{DateTime.UtcNow:yyyyMMddHHmmss}.json";
            return Results.File(bytes, "application/json", fileName);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Auditoria, Permissions.Acoes.Exportar))
        .WithTags("Auditoria")
        .WithName("ExportarLogsAuditoria")
        .Produces(StatusCodes.Status200OK, contentType: "application/json");

        app.MapGet("/api/v1/auditoria/exportar/hash", async (
            [AsParameters] ExportarLogsRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToHashResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Auditoria, Permissions.Acoes.Exportar))
        .WithTags("Auditoria")
        .WithName("ExportarLogsHash")
        .Produces<ExportarLogsHashResponse>();
    }
}
