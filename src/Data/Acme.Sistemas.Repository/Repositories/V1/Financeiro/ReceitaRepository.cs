using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class ReceitaRepository : BaseRepository<Receita>, IReceitaRepository
{
    public ReceitaRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "receitas";
    protected override Func<IDataRecord, Receita> Map => MapReceita;

    public override Task AddAsync(Receita r, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@id"] = r.Id,
            ["@tenant_id"] = TenantContext.TenantId,
            ["@nome"] = r.Nome,
            ["@descricao"] = r.Descricao,
            ["@categoria"] = r.Categoria,
            ["@valor"] = r.Valor,
            ["@receita_fixa"] = r.ReceitaFixa ? 1 : 0,
            ["@data_prevista_recebimento"] = r.DataPrevistaRecebimento,
            ["@competencia_id"] = r.CompetenciaId,
            ["@centro_de_custo_id"] = r.CentroDeCustoId,
            ["@cliente_id"] = r.ClienteId,
            ["@origem_venda_id"] = r.OrigemVendaId,
            ["@status_recebimento"] = (int)r.StatusRecebimento,
            ["@created_at"] = r.CreatedAt,
            ["@created_by"] = r.CreatedBy
        };
        return Db.ExecuteAsync(ReceitaQuery.Insert, parameters, cancellationToken);
    }

    public override Task UpdateAsync(Receita r, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@id"] = r.Id,
            ["@tenantId"] = TenantContext.TenantId,
            ["@nome"] = r.Nome,
            ["@descricao"] = r.Descricao,
            ["@categoria"] = r.Categoria,
            ["@valor"] = r.Valor,
            ["@receita_fixa"] = r.ReceitaFixa ? 1 : 0,
            ["@data_prevista_recebimento"] = r.DataPrevistaRecebimento,
            ["@competencia_id"] = r.CompetenciaId,
            ["@centro_de_custo_id"] = r.CentroDeCustoId,
            ["@cliente_id"] = r.ClienteId,
            ["@origem_venda_id"] = r.OrigemVendaId,
            ["@updated_at"] = DateTime.UtcNow,
            ["@updated_by"] = r.UpdatedBy
        };
        return Db.ExecuteAsync(ReceitaQuery.Update, parameters, cancellationToken);
    }

    public Task ReceberAsync(Receita r, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@id"] = r.Id,
            ["@tenantId"] = TenantContext.TenantId,
            ["@status_recebimento"] = (int)r.StatusRecebimento,
            ["@valor_recebido"] = r.ValorRecebido,
            ["@data_recebimento"] = r.DataRecebimento,
            ["@forma_pagamento"] = r.FormaPagamento.HasValue ? (int)r.FormaPagamento.Value : (int?)null,
            ["@observacao_recebimento"] = r.ObservacaoRecebimento,
            ["@updated_at"] = DateTime.UtcNow,
            ["@updated_by"] = r.UpdatedBy
        };
        return Db.ExecuteAsync(ReceitaQuery.Receber, parameters, cancellationToken);
    }

    public Task<IReadOnlyList<Receita>> ListByFiltroAsync(
        StatusPagamento? status, DateTime? recebimentoInicio, DateTime? recebimentoFim,
        string? categoria, Guid? competenciaId, int skip, int take, CancellationToken cancellationToken = default)
    {
        var (sql, parameters) = BuildFiltroQuery(status, recebimentoInicio, recebimentoFim, categoria, competenciaId);
        sql.Append(" ORDER BY data_prevista_recebimento ASC LIMIT @take OFFSET @skip");
        parameters["@take"] = take;
        parameters["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, parameters, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(
        StatusPagamento? status, DateTime? recebimentoInicio, DateTime? recebimentoFim,
        string? categoria, Guid? competenciaId, CancellationToken cancellationToken = default)
    {
        var (sql, parameters) = BuildFiltroQuery(status, recebimentoInicio, recebimentoFim, categoria, competenciaId, countOnly: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), parameters, cancellationToken);
    }

    private (StringBuilder Sql, Dictionary<string, object?> Parameters) BuildFiltroQuery(
        StatusPagamento? status, DateTime? recebimentoInicio, DateTime? recebimentoFim,
        string? categoria, Guid? competenciaId, bool countOnly = false)
    {
        var sql = new StringBuilder(countOnly
            ? "SELECT COUNT(*) FROM receitas WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {ReceitaQuery.Cols} FROM receitas WHERE tenant_id = @tenantId AND deleted_at IS NULL");

        var parameters = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };

        if (status.HasValue)
        {
            sql.Append(" AND status_recebimento = @status");
            parameters["@status"] = (int)status.Value;
        }
        if (recebimentoInicio.HasValue)
        {
            sql.Append(" AND data_prevista_recebimento >= @inicio");
            parameters["@inicio"] = recebimentoInicio.Value;
        }
        if (recebimentoFim.HasValue)
        {
            sql.Append(" AND data_prevista_recebimento <= @fim");
            parameters["@fim"] = recebimentoFim.Value;
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

    public Task<decimal> SumByPeriodoAsync(DateTime inicio, DateTime fim, bool somenteRecebidas, CancellationToken cancellationToken = default)
    {
        var sql = new StringBuilder(@"SELECT COALESCE(SUM(valor), 0) FROM receitas
            WHERE tenant_id = @tenantId AND deleted_at IS NULL
              AND data_prevista_recebimento >= @inicio AND data_prevista_recebimento <= @fim");
        var parameters = new Dictionary<string, object?>
        {
            ["@tenantId"] = TenantContext.TenantId,
            ["@inicio"] = inicio,
            ["@fim"] = fim
        };
        if (somenteRecebidas)
        {
            sql.Append(" AND status_recebimento = @status");
            parameters["@status"] = (int)StatusPagamento.Pago;
        }
        return Db.ExecuteScalarAsync<decimal>(sql.ToString(), parameters, cancellationToken);
    }

    private static Receita MapReceita(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        Descricao = r.GetValueOrDefault<string>("descricao"),
        Categoria = r.GetValueOrDefault<string>("categoria"),
        Valor = r.GetValueOrDefault<decimal>("valor"),
        ReceitaFixa = r.GetValueOrDefault<int>("receita_fixa") == 1,
        DataPrevistaRecebimento = r.GetValueOrDefault<DateTime>("data_prevista_recebimento"),
        CompetenciaId = r.GetValueOrDefault<Guid?>("competencia_id"),
        CentroDeCustoId = r.GetValueOrDefault<Guid?>("centro_de_custo_id"),
        ClienteId = r.GetValueOrDefault<Guid?>("cliente_id"),
        OrigemVendaId = r.GetValueOrDefault<Guid?>("origem_venda_id"),
        StatusRecebimento = (StatusPagamento)r.GetValueOrDefault<int>("status_recebimento"),
        ValorRecebido = r.GetValueOrDefault<decimal?>("valor_recebido"),
        DataRecebimento = r.GetValueOrDefault<DateTime?>("data_recebimento"),
        FormaPagamento = r.GetValueOrDefault<int?>("forma_pagamento") is int fp ? (FormaPagamento)fp : null,
        ObservacaoRecebimento = r.GetValueOrDefault<string>("observacao_recebimento"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
