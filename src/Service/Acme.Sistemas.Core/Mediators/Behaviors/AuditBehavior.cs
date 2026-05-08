using System.Text.Json;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Domain.Entities.Auditoria;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Core.Mediators.Behaviors;

/// <summary>
/// Persiste <see cref="AuditLog"/> antes/depois para requests que implementam <see cref="IAuditable"/>.
/// Falhas de auditoria são logadas e não interrompem o fluxo principal.
/// </summary>
public sealed class AuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly IAuditLogRepository _audit;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<AuditBehavior<TRequest, TResponse>> _logger;

    public AuditBehavior(
        IAuditLogRepository audit,
        ITenantContext tenantContext,
        ILogger<AuditBehavior<TRequest, TResponse>> logger)
    {
        _audit = audit;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IAuditable auditable) return await next();

        var antesJson = SafeSerialize(request);
        var response = await next();

        try
        {
            var entidadeId = ExtrairId(request);
            var log = new AuditLog
            {
                TenantId = _tenantContext.TenantId,
                UserId = _tenantContext.UserId,
                EntidadeNome = auditable.Recurso,
                EntidadeId = entidadeId,
                Operacao = MapearOperacao(auditable.Acao),
                CommandTipo = typeof(TRequest).FullName ?? typeof(TRequest).Name,
                AntesJson = antesJson,
                DepoisJson = SafeSerialize(response),
                OcorridoEm = DateTime.UtcNow
            };
            await _audit.AddAsync(log, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao persistir audit log para {Tipo}", typeof(TRequest).Name);
        }

        return response;
    }

    private static OperacaoAuditoria MapearOperacao(string acao) => acao switch
    {
        "Criar" => OperacaoAuditoria.Criar,
        "Alterar" => OperacaoAuditoria.Alterar,
        "Excluir" => OperacaoAuditoria.Excluir,
        _ => OperacaoAuditoria.Outro
    };

    private static Guid? ExtrairId(TRequest request)
    {
        var prop = typeof(TRequest).GetProperty("Id");
        if (prop?.PropertyType == typeof(Guid) && prop.GetValue(request) is Guid g && g != Guid.Empty)
            return g;
        return null;
    }

    private static string SafeSerialize(object? obj)
    {
        if (obj is null) return "null";
        try { return JsonSerializer.Serialize(obj, JsonOptions); }
        catch { return "{\"_error\":\"serialization-failed\"}"; }
    }
}
