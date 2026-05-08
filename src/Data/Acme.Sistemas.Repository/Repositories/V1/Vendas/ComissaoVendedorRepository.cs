using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Vendas;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Vendas;

public sealed class ComissaoVendedorRepository : BaseRepository<ComissaoVendedor>, IComissaoVendedorRepository
{
    public ComissaoVendedorRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "comissoes_vendedor";
    protected override Func<IDataRecord, ComissaoVendedor> Map => MapEntity;

    private const string Cols = @"id, tenant_id, vendedor_id, faturamento_id, base_calculo_valor,
        percentual_comissao, valor_comissao, data_referencia, status, data_pagamento,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(ComissaoVendedor c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO comissoes_vendedor
            (id, tenant_id, vendedor_id, faturamento_id, base_calculo_valor,
             percentual_comissao, valor_comissao, data_referencia, status,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @vend, @fat, @base, @perc, @valor, @data, @status, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = c.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@vend"] = c.VendedorId,
                ["@fat"] = c.FaturamentoId,
                ["@base"] = c.BaseCalculoValor,
                ["@perc"] = c.PercentualComissao,
                ["@valor"] = c.ValorComissao,
                ["@data"] = c.DataReferencia,
                ["@status"] = (int)c.Status,
                ["@created_at"] = c.CreatedAt,
                ["@created_by"] = c.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(ComissaoVendedor c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE comissoes_vendedor SET
                status = @status, data_pagamento = @data_pag,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = c.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@status"] = (int)c.Status,
                ["@data_pag"] = c.DataPagamento,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = c.UpdatedBy
            }, cancellationToken);

    public Task<IReadOnlyList<ComissaoVendedor>> ListByVendedorAsync(Guid vendedorId, DateTime? inicio, DateTime? fim, CancellationToken cancellationToken = default)
    {
        var sql = new StringBuilder($"SELECT {Cols} FROM comissoes_vendedor WHERE tenant_id = @tenantId AND vendedor_id = @v AND deleted_at IS NULL");
        var p = new Dictionary<string, object?>
        {
            ["@tenantId"] = TenantContext.TenantId,
            ["@v"] = vendedorId
        };
        if (inicio.HasValue) { sql.Append(" AND data_referencia >= @ini"); p["@ini"] = inicio.Value; }
        if (fim.HasValue) { sql.Append(" AND data_referencia <= @fim"); p["@fim"] = fim.Value; }
        sql.Append(" ORDER BY data_referencia DESC");
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<IReadOnlyList<ComissaoVendedor>> ListByFaturamentoAsync(Guid faturamentoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {Cols} FROM comissoes_vendedor WHERE tenant_id = @tenantId AND faturamento_id = @fid AND deleted_at IS NULL",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@fid"] = faturamentoId },
            cancellationToken);

    private static ComissaoVendedor MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        VendedorId = r.GetValueOrDefault<Guid>("vendedor_id"),
        FaturamentoId = r.GetValueOrDefault<Guid>("faturamento_id"),
        BaseCalculoValor = r.GetValueOrDefault<decimal>("base_calculo_valor"),
        PercentualComissao = r.GetValueOrDefault<decimal>("percentual_comissao"),
        ValorComissao = r.GetValueOrDefault<decimal>("valor_comissao"),
        DataReferencia = r.GetValueOrDefault<DateTime>("data_referencia"),
        Status = (StatusComissao)r.GetValueOrDefault<int>("status"),
        DataPagamento = r.GetValueOrDefault<DateTime?>("data_pagamento"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
