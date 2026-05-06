using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class ContaPagarRepository : BaseRepository<ContaPagar>, IContaPagarRepository
{
    public ContaPagarRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "contas_pagar";
    protected override Func<IDataRecord, ContaPagar> Map => MapEntity;

    private const string Cols = @"id, tenant_id, descricao, fornecedor_id, despesa_id, plano_de_contas_id,
        valor_original, valor_pago, data_vencimento, data_pagamento, status, observacao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(ContaPagar c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO contas_pagar
            (id, tenant_id, descricao, fornecedor_id, despesa_id, plano_de_contas_id,
             valor_original, valor_pago, data_vencimento, data_pagamento, status, observacao,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @descricao, @fornecedor_id, @despesa_id, @plano_de_contas_id,
             @valor_original, @valor_pago, @data_vencimento, @data_pagamento, @status, @observacao,
             @created_at, @created_by)",
            BuildParams(c, isInsert: true), cancellationToken);

    public override Task UpdateAsync(ContaPagar c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE contas_pagar SET
                descricao = @descricao, fornecedor_id = @fornecedor_id,
                despesa_id = @despesa_id, plano_de_contas_id = @plano_de_contas_id,
                valor_original = @valor_original,
                data_vencimento = @data_vencimento, observacao = @observacao,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            BuildParams(c, isInsert: false), cancellationToken);

    public Task BaixarAsync(ContaPagar c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE contas_pagar SET
                valor_pago = @valor_pago,
                data_pagamento = @data_pagamento,
                status = @status,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = c.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@valor_pago"] = c.ValorPago,
                ["@data_pagamento"] = c.DataPagamento,
                ["@status"] = (int)c.Status,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = c.UpdatedBy
            }, cancellationToken);

    public Task<IReadOnlyList<ContaPagar>> ListByFiltroAsync(
        StatusConta? status, DateTime? vencimentoInicio, DateTime? vencimentoFim,
        Guid? fornecedorId, bool somenteVencendoEmAteSeteDias, int skip, int take,
        CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(status, vencimentoInicio, vencimentoFim, fornecedorId, somenteVencendoEmAteSeteDias);
        sql.Append(" ORDER BY data_vencimento ASC LIMIT @take OFFSET @skip");
        p["@take"] = take;
        p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(
        StatusConta? status, DateTime? vencimentoInicio, DateTime? vencimentoFim,
        Guid? fornecedorId, bool somenteVencendoEmAteSeteDias,
        CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(status, vencimentoInicio, vencimentoFim, fornecedorId, somenteVencendoEmAteSeteDias, countOnly: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    private (StringBuilder, Dictionary<string, object?>) BuildFiltro(
        StatusConta? status, DateTime? vencimentoInicio, DateTime? vencimentoFim,
        Guid? fornecedorId, bool somenteVencendoEmAteSeteDias, bool countOnly = false)
    {
        var sql = new StringBuilder(countOnly
            ? "SELECT COUNT(*) FROM contas_pagar WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM contas_pagar WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };

        if (status.HasValue) { sql.Append(" AND status = @status"); p["@status"] = (int)status.Value; }
        if (vencimentoInicio.HasValue) { sql.Append(" AND data_vencimento >= @inicio"); p["@inicio"] = vencimentoInicio.Value; }
        if (vencimentoFim.HasValue) { sql.Append(" AND data_vencimento <= @fim"); p["@fim"] = vencimentoFim.Value; }
        if (fornecedorId.HasValue) { sql.Append(" AND fornecedor_id = @fornecedorId"); p["@fornecedorId"] = fornecedorId.Value; }
        if (somenteVencendoEmAteSeteDias)
        {
            sql.Append(" AND status = @sP AND data_vencimento BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL 7 DAY)");
            p["@sP"] = (int)StatusConta.Pendente;
        }

        return (sql, p);
    }

    private Dictionary<string, object?> BuildParams(ContaPagar c, bool isInsert)
    {
        var p = new Dictionary<string, object?>
        {
            ["@id"] = c.Id,
            ["@descricao"] = c.Descricao,
            ["@fornecedor_id"] = c.FornecedorId,
            ["@despesa_id"] = c.DespesaId,
            ["@plano_de_contas_id"] = c.PlanoDeContasId,
            ["@valor_original"] = c.ValorOriginal,
            ["@valor_pago"] = c.ValorPago,
            ["@data_vencimento"] = c.DataVencimento,
            ["@data_pagamento"] = c.DataPagamento,
            ["@status"] = (int)c.Status,
            ["@observacao"] = c.Observacao
        };
        if (isInsert)
        {
            p["@tenant_id"] = TenantContext.TenantId;
            p["@created_at"] = c.CreatedAt;
            p["@created_by"] = c.CreatedBy;
        }
        else
        {
            p["@tenantId"] = TenantContext.TenantId;
            p["@updated_at"] = DateTime.UtcNow;
            p["@updated_by"] = c.UpdatedBy;
        }
        return p;
    }

    private static ContaPagar MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Descricao = r.GetValueOrDefault<string>("descricao") ?? string.Empty,
        FornecedorId = r.GetValueOrDefault<Guid?>("fornecedor_id"),
        DespesaId = r.GetValueOrDefault<Guid?>("despesa_id"),
        PlanoDeContasId = r.GetValueOrDefault<Guid?>("plano_de_contas_id"),
        ValorOriginal = r.GetValueOrDefault<decimal>("valor_original"),
        ValorPago = r.GetValueOrDefault<decimal>("valor_pago"),
        DataVencimento = r.GetValueOrDefault<DateTime>("data_vencimento"),
        DataPagamento = r.GetValueOrDefault<DateTime?>("data_pagamento"),
        Status = (StatusConta)r.GetValueOrDefault<int>("status"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
