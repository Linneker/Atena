using System.Text.Json;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Domain.Entities.Auditoria;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Services.Behaviors;

/// <summary>
/// Captura comandos de escrita (Criar*/Alterar*/Excluir*/Baixar*/Receber*/Confirmar*/Faturar*/Emitir*/Cancelar*/Registrar*)
/// e persiste log de auditoria após execução bem-sucedida.
/// </summary>
public sealed class AuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private static readonly string[] PrefixosEscrita =
    {
        "Criar", "Alterar", "Excluir", "Baixar", "Receber",
        "Confirmar", "Faturar", "Registrar", "Emitir", "Cancelar",
        "Importar", "Aprovar", "Rejeitar", "Enviar", "Atualizar",
        "Fechar", "Abrir", "Definir", "Vincular"
    };

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

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var tipo = typeof(TRequest);
        var tipoNome = tipo.Name;
        var operacao = DetectarOperacao(tipoNome);
        if (!operacao.HasValue) return await next();

        // Snapshot "antes" do request (Command). Em comandos de Alterar/Excluir, o request contém o ID;
        // não temos ainda o estado atual. Manter simples: serializa o request como "depois".
        var response = await next();

        try
        {
            var (entidade, entidadeId) = ExtrairEntidade(tipoNome, request);
            var depoisJson = SafeSerialize(request);

            var log = new AuditLog
            {
                TenantId = _tenantContext.TenantId,
                UserId = _tenantContext.UserId,
                EntidadeNome = entidade,
                EntidadeId = entidadeId,
                Operacao = operacao.Value,
                CommandTipo = tipo.FullName ?? tipoNome,
                DepoisJson = depoisJson,
                OcorridoEm = DateTime.UtcNow
            };
            await _audit.AddAsync(log, cancellationToken);
        }
        catch (Exception ex)
        {
            // Auditoria não deve quebrar o fluxo principal.
            _logger.LogError(ex, "Falha ao persistir audit log para {Tipo}", tipoNome);
        }

        return response;
    }

    private static OperacaoAuditoria? DetectarOperacao(string tipoNome)
    {
        if (tipoNome.StartsWith("Criar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Importar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Registrar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Abrir", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Emitir", StringComparison.Ordinal))
            return OperacaoAuditoria.Criar;

        if (tipoNome.StartsWith("Alterar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Atualizar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Baixar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Receber", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Confirmar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Faturar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Aprovar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Rejeitar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Enviar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Fechar", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Definir", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Vincular", StringComparison.Ordinal))
            return OperacaoAuditoria.Alterar;

        if (tipoNome.StartsWith("Excluir", StringComparison.Ordinal) ||
            tipoNome.StartsWith("Cancelar", StringComparison.Ordinal))
            return OperacaoAuditoria.Excluir;

        // Verifica fallback
        return PrefixosEscrita.Any(p => tipoNome.StartsWith(p, StringComparison.Ordinal))
            ? OperacaoAuditoria.Outro : null;
    }

    private static (string EntidadeNome, Guid? EntidadeId) ExtrairEntidade(string tipoNome, TRequest request)
    {
        // CriarDespesaCommand → Despesa; AlterarDespesaCommand → Despesa
        var nome = tipoNome;
        foreach (var p in PrefixosEscrita)
        {
            if (nome.StartsWith(p, StringComparison.Ordinal))
            {
                nome = nome[p.Length..];
                break;
            }
        }
        if (nome.EndsWith("Command", StringComparison.Ordinal))
            nome = nome[..^"Command".Length];

        // Tenta extrair Id via reflection (propriedade "Id")
        Guid? id = null;
        var prop = typeof(TRequest).GetProperty("Id");
        if (prop?.PropertyType == typeof(Guid) && prop.GetValue(request) is Guid g && g != Guid.Empty)
            id = g;

        return (string.IsNullOrEmpty(nome) ? typeof(TRequest).Name : nome, id);
    }

    private static string SafeSerialize(object obj)
    {
        try { return JsonSerializer.Serialize(obj, JsonOptions); }
        catch { return "{\"_error\":\"serialization-failed\"}"; }
    }
}
