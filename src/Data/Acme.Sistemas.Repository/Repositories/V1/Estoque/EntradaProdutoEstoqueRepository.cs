using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Estoque;

public sealed class EntradaProdutoEstoqueRepository : BaseRepository<EntradaProdutoEstoque>, IEntradaProdutoEstoqueRepository
{
    public EntradaProdutoEstoqueRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "entrada_produto_estoque";
    protected override Func<IDataRecord, EntradaProdutoEstoque> Map => MapEntity;

    private const string Cols = @"id, tenant_id, estoque_id, produto_id, quantidade, quantidade_restante,
        custo_unitario, origem, motivo, fornecedor_id, documento_referencia, data_movimento,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(EntradaProdutoEstoque e, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO entrada_produto_estoque
            (id, tenant_id, estoque_id, produto_id, quantidade, quantidade_restante, custo_unitario,
             origem, motivo, fornecedor_id, documento_referencia, data_movimento,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @eid, @pid, @qtd, @qtdRest, @custo,
             @origem, @motivo, @forn, @doc, @data,
             @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = e.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@eid"] = e.EstoqueId,
                ["@pid"] = e.ProdutoId,
                ["@qtd"] = e.Quantidade,
                ["@qtdRest"] = e.QuantidadeRestante > 0 ? e.QuantidadeRestante : e.Quantidade,
                ["@custo"] = e.CustoUnitario,
                ["@origem"] = (int)e.Origem,
                ["@motivo"] = e.Motivo,
                ["@forn"] = e.FornecedorId,
                ["@doc"] = e.DocumentoReferencia,
                ["@data"] = e.DataMovimento,
                ["@created_at"] = e.CreatedAt,
                ["@created_by"] = e.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(EntradaProdutoEstoque entity, CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("Movimentos de entrada são imutáveis após registro (use ConsumirLoteAsync para FIFO).");

    public Task ConsumirLoteAsync(Guid loteId, decimal quantidade, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE entrada_produto_estoque
            SET quantidade_restante = quantidade_restante - @qtd, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = loteId,
                ["@tenantId"] = TenantContext.TenantId,
                ["@qtd"] = quantidade,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<IReadOnlyList<EntradaProdutoEstoque>> ListLotesAbertosFifoAsync(
        Guid estoqueId, Guid produtoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $@"SELECT {Cols} FROM entrada_produto_estoque
               WHERE tenant_id = @tenantId AND estoque_id = @eid AND produto_id = @pid
                 AND deleted_at IS NULL AND quantidade_restante > 0
               ORDER BY data_movimento ASC, created_at ASC",
            Map,
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@eid"] = estoqueId,
                ["@pid"] = produtoId
            }, cancellationToken);

    public Task<IReadOnlyList<EntradaProdutoEstoque>> ListByProdutoAsync(
        Guid produtoId, DateTime? inicio, DateTime? fim, int skip, int take,
        CancellationToken cancellationToken = default)
    {
        var sql = new StringBuilder($@"SELECT {Cols} FROM entrada_produto_estoque
            WHERE tenant_id = @tenantId AND produto_id = @pid AND deleted_at IS NULL");
        var p = new Dictionary<string, object?>
        {
            ["@tenantId"] = TenantContext.TenantId,
            ["@pid"] = produtoId
        };
        if (inicio.HasValue) { sql.Append(" AND data_movimento >= @inicio"); p["@inicio"] = inicio.Value; }
        if (fim.HasValue) { sql.Append(" AND data_movimento <= @fim"); p["@fim"] = fim.Value; }
        sql.Append(" ORDER BY data_movimento DESC LIMIT @take OFFSET @skip");
        p["@take"] = take;
        p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    private static EntradaProdutoEstoque MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        EstoqueId = r.GetValueOrDefault<Guid>("estoque_id"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        Quantidade = r.GetValueOrDefault<decimal>("quantidade"),
        QuantidadeRestante = r.GetValueOrDefault<decimal>("quantidade_restante"),
        CustoUnitario = r.GetValueOrDefault<decimal?>("custo_unitario"),
        Origem = (OrigemMovimento)r.GetValueOrDefault<int>("origem"),
        Motivo = r.GetValueOrDefault<string>("motivo"),
        FornecedorId = r.GetValueOrDefault<Guid?>("fornecedor_id"),
        DocumentoReferencia = r.GetValueOrDefault<string>("documento_referencia"),
        DataMovimento = r.GetValueOrDefault<DateTime>("data_movimento"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
