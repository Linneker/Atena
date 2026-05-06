using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class DividaRepository : BaseRepository<Divida>, IDividaRepository
{
    public DividaRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "dividas";
    protected override Func<IDataRecord, Divida> Map => MapEntity;

    private const string Cols = @"id, tenant_id, credor, descricao, valor_original, valor_pago,
        taxa_juros_mensal, data_inicio, data_fim, numero_parcelas, status,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(Divida d, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO dividas
            (id, tenant_id, credor, descricao, valor_original, valor_pago,
             taxa_juros_mensal, data_inicio, data_fim, numero_parcelas, status,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @credor, @descricao, @valor_original, @valor_pago,
             @taxa, @data_inicio, @data_fim, @parcelas, @status,
             @created_at, @created_by)",
            BuildParams(d, isInsert: true), cancellationToken);

    public override Task UpdateAsync(Divida d, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE dividas SET
                credor = @credor, descricao = @descricao,
                valor_original = @valor_original, valor_pago = @valor_pago,
                taxa_juros_mensal = @taxa, data_inicio = @data_inicio, data_fim = @data_fim,
                numero_parcelas = @parcelas, status = @status,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            BuildParams(d, isInsert: false), cancellationToken);

    public Task<IReadOnlyList<Divida>> ListByFiltroAsync(StatusConta? status, int skip, int take, CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(status);
        sql.Append(" ORDER BY data_inicio DESC LIMIT @take OFFSET @skip");
        p["@take"] = take;
        p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(StatusConta? status, CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(status, countOnly: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    private (StringBuilder, Dictionary<string, object?>) BuildFiltro(StatusConta? status, bool countOnly = false)
    {
        var sql = new StringBuilder(countOnly
            ? "SELECT COUNT(*) FROM dividas WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM dividas WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        if (status.HasValue)
        {
            sql.Append(" AND status = @status");
            p["@status"] = (int)status.Value;
        }
        return (sql, p);
    }

    private Dictionary<string, object?> BuildParams(Divida d, bool isInsert)
    {
        var p = new Dictionary<string, object?>
        {
            ["@id"] = d.Id,
            ["@credor"] = d.Credor,
            ["@descricao"] = d.Descricao,
            ["@valor_original"] = d.ValorOriginal,
            ["@valor_pago"] = d.ValorPago,
            ["@taxa"] = d.TaxaJurosMensal,
            ["@data_inicio"] = d.DataInicio,
            ["@data_fim"] = d.DataFim,
            ["@parcelas"] = d.NumeroParcelas,
            ["@status"] = (int)d.Status
        };
        if (isInsert)
        {
            p["@tenant_id"] = TenantContext.TenantId;
            p["@created_at"] = d.CreatedAt;
            p["@created_by"] = d.CreatedBy;
        }
        else
        {
            p["@tenantId"] = TenantContext.TenantId;
            p["@updated_at"] = DateTime.UtcNow;
            p["@updated_by"] = d.UpdatedBy;
        }
        return p;
    }

    private static Divida MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Credor = r.GetValueOrDefault<string>("credor") ?? string.Empty,
        Descricao = r.GetValueOrDefault<string>("descricao"),
        ValorOriginal = r.GetValueOrDefault<decimal>("valor_original"),
        ValorPago = r.GetValueOrDefault<decimal>("valor_pago"),
        TaxaJurosMensal = r.GetValueOrDefault<decimal?>("taxa_juros_mensal"),
        DataInicio = r.GetValueOrDefault<DateTime>("data_inicio"),
        DataFim = r.GetValueOrDefault<DateTime?>("data_fim"),
        NumeroParcelas = r.GetValueOrDefault<int>("numero_parcelas"),
        Status = (StatusConta)r.GetValueOrDefault<int>("status"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
