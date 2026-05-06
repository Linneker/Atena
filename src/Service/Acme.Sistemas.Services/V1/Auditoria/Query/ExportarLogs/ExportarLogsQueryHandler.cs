using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.ExportarLogs;

public sealed class ExportarLogsQueryHandler
    : IRequestHandler<ExportarLogsQuery, ResponseDefault<ExportarLogsQueryResult>>
{
    private readonly IAuditLogRepository _repo;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExportarLogsQueryHandler(IAuditLogRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ExportarLogsQueryResult>> Handle(ExportarLogsQuery request, CancellationToken cancellationToken)
    {
        // Coleta paginada (sem limite externo, mas com batch para evitar OOM)
        var todos = new List<Domain.Entities.Auditoria.AuditLog>();
        const int batch = 1000;
        int skip = 0;
        while (true)
        {
            var page = await _repo.ListAsync(
                request.UserId, request.Entidade, request.Operacao,
                request.Inicio, request.Fim, skip, batch, cancellationToken);
            if (page.Count == 0) break;
            todos.AddRange(page);
            if (page.Count < batch) break;
            skip += batch;
        }

        var payload = new
        {
            exportadoEm = DateTime.UtcNow,
            filtros = new
            {
                userId = request.UserId,
                entidade = request.Entidade,
                operacao = request.Operacao?.ToString(),
                inicio = request.Inicio,
                fim = request.Fim
            },
            totalRegistros = todos.Count,
            registros = todos.Select(l => new
            {
                id = l.Id,
                tenantId = l.TenantId,
                userId = l.UserId,
                entidade = l.EntidadeNome,
                entidadeId = l.EntidadeId,
                operacao = l.Operacao.ToString(),
                commandTipo = l.CommandTipo,
                antes = l.AntesJson,
                depois = l.DepoisJson,
                ocorridoEm = l.OcorridoEm
            })
        };

        var json = JsonSerializer.Serialize(payload, JsonOptions);
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(json))).ToLowerInvariant();

        return ResponseDefault<ExportarLogsQueryResult>.Ok(
            new ExportarLogsQueryResult(todos.Count, hash, json));
    }
}
