using System.Data;
using Acme.Sistemas.Domain.Entities.Compras;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Compras;

public sealed class RecebimentoCompraRepository : BaseRepository<RecebimentoCompra>, IRecebimentoCompraRepository
{
    public RecebimentoCompraRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "recebimentos_compra";
    protected override Func<IDataRecord, RecebimentoCompra> Map => MapEntity;

    private const string Cols = @"id, tenant_id, pedido_compra_id, data_recebimento, tipo,
        numero_nota_fiscal, chave_acesso_nfe, observacao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string ItemCols = @"id, tenant_id, recebimento_compra_id, pedido_compra_item_id,
        produto_id, quantidade_recebida, preco_unitario, observacao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(RecebimentoCompra r, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO recebimentos_compra
            (id, tenant_id, pedido_compra_id, data_recebimento, tipo,
             numero_nota_fiscal, chave_acesso_nfe, observacao,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @ped, @data, @tipo, @nnf, @chave, @obs, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = r.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@ped"] = r.PedidoCompraId,
                ["@data"] = r.DataRecebimento,
                ["@tipo"] = (int)r.Tipo,
                ["@nnf"] = r.NumeroNotaFiscal,
                ["@chave"] = r.ChaveAcessoNFe,
                ["@obs"] = r.Observacao,
                ["@created_at"] = r.CreatedAt,
                ["@created_by"] = r.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(RecebimentoCompra entity, CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("Use VincularNFeAsync para atualizar NF-e.");

    public Task VincularNFeAsync(Guid recebimentoId, string numeroNotaFiscal, string chaveAcesso, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE recebimentos_compra
            SET numero_nota_fiscal = @nnf, chave_acesso_nfe = @chave, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = recebimentoId,
                ["@tenantId"] = TenantContext.TenantId,
                ["@nnf"] = numeroNotaFiscal,
                ["@chave"] = chaveAcesso,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<IReadOnlyList<RecebimentoCompra>> ListByPedidoAsync(Guid pedidoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $@"SELECT {Cols} FROM recebimentos_compra
               WHERE tenant_id = @tenantId AND pedido_compra_id = @pid AND deleted_at IS NULL
               ORDER BY data_recebimento DESC",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@pid"] = pedidoId },
            cancellationToken);

    public Task<IReadOnlyList<RecebimentoCompraItem>> ListItensAsync(Guid recebimentoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $@"SELECT {ItemCols} FROM recebimento_compra_itens
               WHERE tenant_id = @tenantId AND recebimento_compra_id = @rid AND deleted_at IS NULL",
            MapItem,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@rid"] = recebimentoId },
            cancellationToken);

    public async Task AddItensAsync(IEnumerable<RecebimentoCompraItem> itens, CancellationToken cancellationToken = default)
    {
        foreach (var i in itens)
        {
            await Db.ExecuteAsync(@"
                INSERT INTO recebimento_compra_itens
                (id, tenant_id, recebimento_compra_id, pedido_compra_item_id, produto_id,
                 quantidade_recebida, preco_unitario, observacao,
                 created_at, created_by)
                VALUES
                (@id, @tenant_id, @rid, @pcid, @prod, @qtd, @preco, @obs, @created_at, @created_by)",
                new Dictionary<string, object?>
                {
                    ["@id"] = i.Id,
                    ["@tenant_id"] = TenantContext.TenantId,
                    ["@rid"] = i.RecebimentoCompraId,
                    ["@pcid"] = i.PedidoCompraItemId,
                    ["@prod"] = i.ProdutoId,
                    ["@qtd"] = i.QuantidadeRecebida,
                    ["@preco"] = i.PrecoUnitario,
                    ["@obs"] = i.Observacao,
                    ["@created_at"] = i.CreatedAt,
                    ["@created_by"] = i.CreatedBy
                }, cancellationToken);
        }
    }

    public Task UpdatePedidoCompraItemQuantidadeRecebidaAsync(Guid pedidoCompraItemId, decimal novaQuantidadeRecebida, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE pedido_compra_itens SET quantidade_recebida = @qrec, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = pedidoCompraItemId,
                ["@tenantId"] = TenantContext.TenantId,
                ["@qrec"] = novaQuantidadeRecebida,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    private static RecebimentoCompra MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        PedidoCompraId = r.GetValueOrDefault<Guid>("pedido_compra_id"),
        DataRecebimento = r.GetValueOrDefault<DateTime>("data_recebimento"),
        Tipo = (TipoRecebimento)r.GetValueOrDefault<int>("tipo"),
        NumeroNotaFiscal = r.GetValueOrDefault<string>("numero_nota_fiscal"),
        ChaveAcessoNFe = r.GetValueOrDefault<string>("chave_acesso_nfe"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static RecebimentoCompraItem MapItem(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        RecebimentoCompraId = r.GetValueOrDefault<Guid>("recebimento_compra_id"),
        PedidoCompraItemId = r.GetValueOrDefault<Guid>("pedido_compra_item_id"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        QuantidadeRecebida = r.GetValueOrDefault<decimal>("quantidade_recebida"),
        PrecoUnitario = r.GetValueOrDefault<decimal?>("preco_unitario"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
