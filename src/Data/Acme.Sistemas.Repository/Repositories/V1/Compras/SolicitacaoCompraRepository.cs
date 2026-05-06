using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Compras;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Compras;

public sealed class SolicitacaoCompraRepository : BaseRepository<SolicitacaoCompra>, ISolicitacaoCompraRepository
{
    public SolicitacaoCompraRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "solicitacoes_compra";
    protected override Func<IDataRecord, SolicitacaoCompra> Map => MapEntity;

    private const string Cols = @"id, tenant_id, numero, solicitante_id, justificativa, valor_total,
        data_solicitacao, status, aprovado_por, aprovado_em, motivo_rejeicao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string ItemCols = @"id, tenant_id, solicitacao_compra_id, produto_id,
        quantidade, preco_estimado, observacao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(SolicitacaoCompra s, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO solicitacoes_compra
            (id, tenant_id, numero, solicitante_id, justificativa, valor_total,
             data_solicitacao, status, created_at, created_by)
            VALUES
            (@id, @tenant_id, @numero, @sol, @just, @valor,
             @data, @status, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = s.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@numero"] = s.Numero,
                ["@sol"] = s.SolicitanteId,
                ["@just"] = s.Justificativa,
                ["@valor"] = s.ValorTotal,
                ["@data"] = s.DataSolicitacao,
                ["@status"] = (int)s.Status,
                ["@created_at"] = s.CreatedAt,
                ["@created_by"] = s.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(SolicitacaoCompra s, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE solicitacoes_compra SET
                solicitante_id = @sol, justificativa = @just, valor_total = @valor,
                status = @status, updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = s.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@sol"] = s.SolicitanteId,
                ["@just"] = s.Justificativa,
                ["@valor"] = s.ValorTotal,
                ["@status"] = (int)s.Status,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = s.UpdatedBy
            }, cancellationToken);

    public Task UpdateStatusAsync(Guid id, StatusSolicitacaoCompra status, Guid? aprovadoPor, DateTime? aprovadoEm, string? motivoRejeicao, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE solicitacoes_compra SET
                status = @status, aprovado_por = @ap, aprovado_em = @em,
                motivo_rejeicao = @mr, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@status"] = (int)status,
                ["@ap"] = aprovadoPor,
                ["@em"] = aprovadoEm,
                ["@mr"] = motivoRejeicao,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task UpdateStatusOnlyAsync(Guid id, StatusSolicitacaoCompra status, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE solicitacoes_compra SET status = @status, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@status"] = (int)status,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<SolicitacaoCompra?> GetByNumeroAsync(string numero, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM solicitacoes_compra WHERE tenant_id = @tenantId AND numero = @num AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@num"] = numero },
            cancellationToken);

    public Task<IReadOnlyList<SolicitacaoCompra>> ListByFiltroAsync(StatusSolicitacaoCompra? status, int skip, int take, CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(status);
        sql.Append(" ORDER BY data_solicitacao DESC LIMIT @take OFFSET @skip");
        p["@take"] = take;
        p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(StatusSolicitacaoCompra? status, CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(status, countOnly: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    private (StringBuilder, Dictionary<string, object?>) BuildFiltro(StatusSolicitacaoCompra? status, bool countOnly = false)
    {
        var sql = new StringBuilder(countOnly
            ? "SELECT COUNT(*) FROM solicitacoes_compra WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM solicitacoes_compra WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        if (status.HasValue)
        {
            sql.Append(" AND status = @s");
            p["@s"] = (int)status.Value;
        }
        return (sql, p);
    }

    public Task<IReadOnlyList<SolicitacaoCompraItem>> ListItensAsync(Guid solicitacaoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {ItemCols} FROM solicitacao_compra_itens WHERE tenant_id = @tenantId AND solicitacao_compra_id = @sid AND deleted_at IS NULL",
            MapItem,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@sid"] = solicitacaoId },
            cancellationToken);

    public async Task AddItensAsync(IEnumerable<SolicitacaoCompraItem> itens, CancellationToken cancellationToken = default)
    {
        foreach (var i in itens)
        {
            await Db.ExecuteAsync(@"
                INSERT INTO solicitacao_compra_itens
                (id, tenant_id, solicitacao_compra_id, produto_id, quantidade, preco_estimado, observacao,
                 created_at, created_by)
                VALUES (@id, @tenant_id, @sid, @pid, @qtd, @preco, @obs, @created_at, @created_by)",
                new Dictionary<string, object?>
                {
                    ["@id"] = i.Id,
                    ["@tenant_id"] = TenantContext.TenantId,
                    ["@sid"] = i.SolicitacaoCompraId,
                    ["@pid"] = i.ProdutoId,
                    ["@qtd"] = i.Quantidade,
                    ["@preco"] = i.PrecoEstimado,
                    ["@obs"] = i.Observacao,
                    ["@created_at"] = i.CreatedAt,
                    ["@created_by"] = i.CreatedBy
                }, cancellationToken);
        }
    }

    public Task RemoveItensAsync(Guid solicitacaoId, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE solicitacao_compra_itens SET deleted_at = @now
            WHERE tenant_id = @tenantId AND solicitacao_compra_id = @sid AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@sid"] = solicitacaoId,
                ["@now"] = DateTime.UtcNow
            }, cancellationToken);

    public async Task<int> NextNumeroAsync(CancellationToken cancellationToken = default)
    {
        var count = await Db.ExecuteScalarAsync<long>(
            "SELECT COUNT(*) FROM solicitacoes_compra WHERE tenant_id = @tenantId",
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId },
            cancellationToken);
        return (int)count + 1;
    }

    private static SolicitacaoCompra MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Numero = r.GetValueOrDefault<string>("numero") ?? string.Empty,
        SolicitanteId = r.GetValueOrDefault<Guid?>("solicitante_id"),
        Justificativa = r.GetValueOrDefault<string>("justificativa"),
        ValorTotal = r.GetValueOrDefault<decimal>("valor_total"),
        DataSolicitacao = r.GetValueOrDefault<DateTime>("data_solicitacao"),
        Status = (StatusSolicitacaoCompra)r.GetValueOrDefault<int>("status"),
        AprovadoPor = r.GetValueOrDefault<Guid?>("aprovado_por"),
        AprovadoEm = r.GetValueOrDefault<DateTime?>("aprovado_em"),
        MotivoRejeicao = r.GetValueOrDefault<string>("motivo_rejeicao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static SolicitacaoCompraItem MapItem(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        SolicitacaoCompraId = r.GetValueOrDefault<Guid>("solicitacao_compra_id"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        Quantidade = r.GetValueOrDefault<decimal>("quantidade"),
        PrecoEstimado = r.GetValueOrDefault<decimal?>("preco_estimado"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
