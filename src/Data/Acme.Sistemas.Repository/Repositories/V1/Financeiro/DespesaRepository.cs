using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class DespesaRepository : BaseRepository<Despesa>, IDespesaRepository
{
    public DespesaRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "despesas";
    protected override Func<IDataRecord, Despesa> Map => MapDespesa;

    public override Task AddAsync(Despesa d, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@id"] = d.Id,
            ["@tenant_id"] = TenantContext.TenantId,
            ["@nome"] = d.Nome,
            ["@descricao"] = d.Descricao,
            ["@categoria"] = d.Categoria,
            ["@valor"] = d.Valor,
            ["@despesa_fixa"] = d.DespesaFixa ? 1 : 0,
            ["@data_vencimento"] = d.DataVencimento,
            ["@competencia_id"] = d.CompetenciaId,
            ["@centro_de_custo_id"] = d.CentroDeCustoId,
            ["@fornecedor_id"] = d.FornecedorId,
            ["@origem_despesa_id"] = d.OrigemDespesaId,
            ["@status_pagamento"] = (int)d.StatusPagamento,
            ["@created_at"] = d.CreatedAt,
            ["@created_by"] = d.CreatedBy
        };
        return Db.ExecuteAsync(DespesaQuery.Insert, parameters, cancellationToken);
    }

    public override Task UpdateAsync(Despesa d, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@id"] = d.Id,
            ["@tenantId"] = TenantContext.TenantId,
            ["@nome"] = d.Nome,
            ["@descricao"] = d.Descricao,
            ["@categoria"] = d.Categoria,
            ["@valor"] = d.Valor,
            ["@despesa_fixa"] = d.DespesaFixa ? 1 : 0,
            ["@data_vencimento"] = d.DataVencimento,
            ["@competencia_id"] = d.CompetenciaId,
            ["@centro_de_custo_id"] = d.CentroDeCustoId,
            ["@fornecedor_id"] = d.FornecedorId,
            ["@updated_at"] = DateTime.UtcNow,
            ["@updated_by"] = d.UpdatedBy
        };
        return Db.ExecuteAsync(DespesaQuery.Update, parameters, cancellationToken);
    }

    public Task BaixarAsync(Despesa d, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@id"] = d.Id,
            ["@tenantId"] = TenantContext.TenantId,
            ["@status_pagamento"] = (int)d.StatusPagamento,
            ["@valor_pago"] = d.ValorPago,
            ["@data_pagamento"] = d.DataPagamento,
            ["@forma_pagamento"] = d.FormaPagamento.HasValue ? (int)d.FormaPagamento.Value : (int?)null,
            ["@observacao_pagamento"] = d.ObservacaoPagamento,
            ["@updated_at"] = DateTime.UtcNow,
            ["@updated_by"] = d.UpdatedBy
        };
        return Db.ExecuteAsync(DespesaQuery.Baixar, parameters, cancellationToken);
    }

    public Task<IReadOnlyList<Despesa>> ListByFiltroAsync(
        StatusPagamento? status, DateTime? vencimentoInicio, DateTime? vencimentoFim,
        string? categoria, Guid? competenciaId, int skip, int take, CancellationToken cancellationToken = default)
    {
        var (sql, parameters) = BuildFiltroQuery(status, vencimentoInicio, vencimentoFim, categoria, competenciaId);
        sql.Append(" ORDER BY data_vencimento ASC LIMIT @take OFFSET @skip");
        parameters["@take"] = take;
        parameters["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, parameters, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(
        StatusPagamento? status, DateTime? vencimentoInicio, DateTime? vencimentoFim,
        string? categoria, Guid? competenciaId, CancellationToken cancellationToken = default)
    {
        var (sql, parameters) = BuildFiltroQuery(status, vencimentoInicio, vencimentoFim, categoria, competenciaId, countOnly: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), parameters, cancellationToken);
    }

    private (StringBuilder Sql, Dictionary<string, object?> Parameters) BuildFiltroQuery(
        StatusPagamento? status, DateTime? vencimentoInicio, DateTime? vencimentoFim,
        string? categoria, Guid? competenciaId, bool countOnly = false)
    {
        var sql = new StringBuilder(countOnly
            ? "SELECT COUNT(*) FROM despesas WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {DespesaQuery.Cols} FROM despesas WHERE tenant_id = @tenantId AND deleted_at IS NULL");

        var parameters = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };

        if (status.HasValue)
        {
            sql.Append(" AND status_pagamento = @status");
            parameters["@status"] = (int)status.Value;
        }
        if (vencimentoInicio.HasValue)
        {
            sql.Append(" AND data_vencimento >= @inicio");
            parameters["@inicio"] = vencimentoInicio.Value;
        }
        if (vencimentoFim.HasValue)
        {
            sql.Append(" AND data_vencimento <= @fim");
            parameters["@fim"] = vencimentoFim.Value;
        }
        if (!string.IsNullOrWhiteSpace(categoria))
        {
            sql.Append(" AND categoria = @categoria");
            parameters["@categoria"] = categoria;
        }
        if (competenciaId.HasValue)
        {
            sql.Append(" AND competencia_id = @competenciaId");
            parameters["@competenciaId"] = competenciaId.Value;
        }

        return (sql, parameters);
    }

    public Task<decimal> SumByPeriodoAsync(DateTime inicio, DateTime fim, bool somenteBaixadas, CancellationToken cancellationToken = default)
    {
        var sql = new StringBuilder(@"SELECT COALESCE(SUM(valor), 0) FROM despesas
            WHERE tenant_id = @tenantId AND deleted_at IS NULL
              AND data_vencimento >= @inicio AND data_vencimento <= @fim");
        var parameters = new Dictionary<string, object?>
        {
            ["@tenantId"] = TenantContext.TenantId,
            ["@inicio"] = inicio,
            ["@fim"] = fim
        };
        if (somenteBaixadas)
        {
            sql.Append(" AND status_pagamento = @status");
            parameters["@status"] = (int)StatusPagamento.Pago;
        }
        return Db.ExecuteScalarAsync<decimal>(sql.ToString(), parameters, cancellationToken);
    }

    private static Despesa MapDespesa(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        Descricao = r.GetValueOrDefault<string>("descricao"),
        Categoria = r.GetValueOrDefault<string>("categoria"),
        Valor = r.GetValueOrDefault<decimal>("valor"),
        DespesaFixa = r.GetValueOrDefault<int>("despesa_fixa") == 1,
        DataVencimento = r.GetValueOrDefault<DateTime>("data_vencimento"),
        CompetenciaId = r.GetValueOrDefault<Guid?>("competencia_id"),
        CentroDeCustoId = r.GetValueOrDefault<Guid?>("centro_de_custo_id"),
        FornecedorId = r.GetValueOrDefault<Guid?>("fornecedor_id"),
        OrigemDespesaId = r.GetValueOrDefault<Guid?>("origem_despesa_id"),
        StatusPagamento = (StatusPagamento)r.GetValueOrDefault<int>("status_pagamento"),
        ValorPago = r.GetValueOrDefault<decimal?>("valor_pago"),
        DataPagamento = r.GetValueOrDefault<DateTime?>("data_pagamento"),
        FormaPagamento = r.GetValueOrDefault<int?>("forma_pagamento") is int fp ? (FormaPagamento)fp : null,
        ObservacaoPagamento = r.GetValueOrDefault<string>("observacao_pagamento"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
