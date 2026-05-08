using System.Data;
using Acme.Sistemas.Domain.Entities.Vendas;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Vendas;

public sealed class DevolucaoVendaRepository : BaseRepository<DevolucaoVenda>, IDevolucaoVendaRepository
{
    public DevolucaoVendaRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "devolucoes_venda";
    protected override Func<IDataRecord, DevolucaoVenda> Map => MapEntity;

    private const string Cols = @"id, tenant_id, faturamento_id, data_devolucao, tipo, valor_total,
        motivo, nfe_devolucao_id,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string ItemCols = @"id, tenant_id, devolucao_venda_id, faturamento_item_id,
        produto_id, quantidade, preco_unitario,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(DevolucaoVenda d, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO devolucoes_venda
            (id, tenant_id, faturamento_id, data_devolucao, tipo, valor_total,
             motivo, nfe_devolucao_id, created_at, created_by)
            VALUES (@id, @tenant_id, @fid, @data, @tipo, @valor, @motivo, @nfe, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = d.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@fid"] = d.FaturamentoId,
                ["@data"] = d.DataDevolucao,
                ["@tipo"] = (int)d.Tipo,
                ["@valor"] = d.ValorTotal,
                ["@motivo"] = d.Motivo,
                ["@nfe"] = d.NFeDevolucaoId,
                ["@created_at"] = d.CreatedAt,
                ["@created_by"] = d.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(DevolucaoVenda entity, CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("Devolução é imutável.");

    public Task<IReadOnlyList<DevolucaoVenda>> ListByFaturamentoAsync(Guid faturamentoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {Cols} FROM devolucoes_venda WHERE tenant_id = @tenantId AND faturamento_id = @fid AND deleted_at IS NULL ORDER BY data_devolucao DESC",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@fid"] = faturamentoId },
            cancellationToken);

    public Task<IReadOnlyList<DevolucaoVendaItem>> ListItensAsync(Guid devolucaoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {ItemCols} FROM devolucao_venda_itens WHERE tenant_id = @tenantId AND devolucao_venda_id = @did AND deleted_at IS NULL",
            MapItem,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@did"] = devolucaoId },
            cancellationToken);

    public async Task AddItensAsync(IEnumerable<DevolucaoVendaItem> itens, CancellationToken cancellationToken = default)
    {
        foreach (var i in itens)
        {
            await Db.ExecuteAsync(@"
                INSERT INTO devolucao_venda_itens
                (id, tenant_id, devolucao_venda_id, faturamento_item_id, produto_id,
                 quantidade, preco_unitario, created_at, created_by)
                VALUES (@id, @tenant_id, @did, @fii, @prod, @qtd, @preco, @created_at, @created_by)",
                new Dictionary<string, object?>
                {
                    ["@id"] = i.Id,
                    ["@tenant_id"] = TenantContext.TenantId,
                    ["@did"] = i.DevolucaoVendaId,
                    ["@fii"] = i.FaturamentoItemId,
                    ["@prod"] = i.ProdutoId,
                    ["@qtd"] = i.Quantidade,
                    ["@preco"] = i.PrecoUnitario,
                    ["@created_at"] = i.CreatedAt,
                    ["@created_by"] = i.CreatedBy
                }, cancellationToken);
        }
    }

    private static DevolucaoVenda MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FaturamentoId = r.GetValueOrDefault<Guid>("faturamento_id"),
        DataDevolucao = r.GetValueOrDefault<DateTime>("data_devolucao"),
        Tipo = (TipoDevolucao)r.GetValueOrDefault<int>("tipo"),
        ValorTotal = r.GetValueOrDefault<decimal>("valor_total"),
        Motivo = r.GetValueOrDefault<string>("motivo"),
        NFeDevolucaoId = r.GetValueOrDefault<Guid?>("nfe_devolucao_id"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static DevolucaoVendaItem MapItem(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        DevolucaoVendaId = r.GetValueOrDefault<Guid>("devolucao_venda_id"),
        FaturamentoItemId = r.GetValueOrDefault<Guid>("faturamento_item_id"),
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
