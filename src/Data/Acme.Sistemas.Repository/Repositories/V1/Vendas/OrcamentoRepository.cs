using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Vendas;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Vendas;

public sealed class OrcamentoRepository : BaseRepository<Orcamento>, IOrcamentoRepository
{
    public OrcamentoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "orcamentos";
    protected override Func<IDataRecord, Orcamento> Map => MapEntity;

    private const string Cols = @"id, tenant_id, numero, cliente_id, vendedor_id, data_emissao, data_validade,
        valor_total, desconto_percentual, status, observacao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string ItemCols = @"id, tenant_id, orcamento_id, produto_id, quantidade, preco_unitario,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(Orcamento o, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO orcamentos
            (id, tenant_id, numero, cliente_id, vendedor_id, data_emissao, data_validade,
             valor_total, desconto_percentual, status, observacao, created_at, created_by)
            VALUES
            (@id, @tenant_id, @numero, @cli, @vend, @em, @val, @valor, @desc, @status, @obs, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = o.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@numero"] = o.Numero,
                ["@cli"] = o.ClienteId,
                ["@vend"] = o.VendedorId,
                ["@em"] = o.DataEmissao,
                ["@val"] = o.DataValidade,
                ["@valor"] = o.ValorTotal,
                ["@desc"] = o.DescontoPercentual,
                ["@status"] = (int)o.Status,
                ["@obs"] = o.Observacao,
                ["@created_at"] = o.CreatedAt,
                ["@created_by"] = o.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(Orcamento o, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE orcamentos SET
                cliente_id = @cli, vendedor_id = @vend, data_validade = @val,
                valor_total = @valor, desconto_percentual = @desc, status = @status,
                observacao = @obs, updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = o.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@cli"] = o.ClienteId,
                ["@vend"] = o.VendedorId,
                ["@val"] = o.DataValidade,
                ["@valor"] = o.ValorTotal,
                ["@desc"] = o.DescontoPercentual,
                ["@status"] = (int)o.Status,
                ["@obs"] = o.Observacao,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = o.UpdatedBy
            }, cancellationToken);

    public Task UpdateStatusAsync(Guid id, StatusOrcamento status, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE orcamentos SET status = @s, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@s"] = (int)status,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<IReadOnlyList<Orcamento>> ListByFiltroAsync(StatusOrcamento? status, Guid? clienteId, int skip, int take, CancellationToken cancellationToken = default)
    {
        var (sql, p) = Filtro(status, clienteId);
        sql.Append(" ORDER BY data_emissao DESC LIMIT @take OFFSET @skip");
        p["@take"] = take; p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(StatusOrcamento? status, Guid? clienteId, CancellationToken cancellationToken = default)
    {
        var (sql, p) = Filtro(status, clienteId, count: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    private (StringBuilder, Dictionary<string, object?>) Filtro(StatusOrcamento? status, Guid? clienteId, bool count = false)
    {
        var sql = new StringBuilder(count
            ? "SELECT COUNT(*) FROM orcamentos WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM orcamentos WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        if (status.HasValue) { sql.Append(" AND status = @s"); p["@s"] = (int)status.Value; }
        if (clienteId.HasValue) { sql.Append(" AND cliente_id = @c"); p["@c"] = clienteId.Value; }
        return (sql, p);
    }

    public Task<IReadOnlyList<OrcamentoItem>> ListItensAsync(Guid orcamentoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {ItemCols} FROM orcamento_itens WHERE tenant_id = @tenantId AND orcamento_id = @oid AND deleted_at IS NULL",
            MapItem,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@oid"] = orcamentoId },
            cancellationToken);

    public async Task AddItensAsync(IEnumerable<OrcamentoItem> itens, CancellationToken cancellationToken = default)
    {
        foreach (var i in itens)
        {
            await Db.ExecuteAsync(@"
                INSERT INTO orcamento_itens
                (id, tenant_id, orcamento_id, produto_id, quantidade, preco_unitario, created_at, created_by)
                VALUES (@id, @tenant_id, @oid, @prod, @qtd, @preco, @created_at, @created_by)",
                new Dictionary<string, object?>
                {
                    ["@id"] = i.Id,
                    ["@tenant_id"] = TenantContext.TenantId,
                    ["@oid"] = i.OrcamentoId,
                    ["@prod"] = i.ProdutoId,
                    ["@qtd"] = i.Quantidade,
                    ["@preco"] = i.PrecoUnitario,
                    ["@created_at"] = i.CreatedAt,
                    ["@created_by"] = i.CreatedBy
                }, cancellationToken);
        }
    }

    public async Task<int> NextNumeroAsync(CancellationToken cancellationToken = default)
    {
        var c = await Db.ExecuteScalarAsync<long>(
            "SELECT COUNT(*) FROM orcamentos WHERE tenant_id = @tenantId",
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId }, cancellationToken);
        return (int)c + 1;
    }

    private static Orcamento MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Numero = r.GetValueOrDefault<string>("numero") ?? string.Empty,
        ClienteId = r.GetValueOrDefault<Guid>("cliente_id"),
        VendedorId = r.GetValueOrDefault<Guid?>("vendedor_id"),
        DataEmissao = r.GetValueOrDefault<DateTime>("data_emissao"),
        DataValidade = r.GetValueOrDefault<DateTime>("data_validade"),
        ValorTotal = r.GetValueOrDefault<decimal>("valor_total"),
        DescontoPercentual = r.GetValueOrDefault<decimal?>("desconto_percentual"),
        Status = (StatusOrcamento)r.GetValueOrDefault<int>("status"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static OrcamentoItem MapItem(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        OrcamentoId = r.GetValueOrDefault<Guid>("orcamento_id"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        Quantidade = r.GetValueOrDefault<decimal>("quantidade"),
        PrecoUnitario = r.GetValueOrDefault<decimal>("preco_unitario"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
