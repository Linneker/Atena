using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Estoque;

public sealed class SaidaProdutoEstoqueRepository : BaseRepository<SaidaProdutoEstoque>, ISaidaProdutoEstoqueRepository
{
    public SaidaProdutoEstoqueRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "saida_produto_estoque";
    protected override Func<IDataRecord, SaidaProdutoEstoque> Map => MapEntity;

    private const string Cols = @"id, tenant_id, estoque_id, produto_id, quantidade, custo_unitario, cmv_unitario,
        origem, motivo, cliente_id, documento_referencia, data_movimento,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(SaidaProdutoEstoque s, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO saida_produto_estoque
            (id, tenant_id, estoque_id, produto_id, quantidade, custo_unitario, cmv_unitario,
             origem, motivo, cliente_id, documento_referencia, data_movimento,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @eid, @pid, @qtd, @custo, @cmv,
             @origem, @motivo, @cli, @doc, @data,
             @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = s.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@eid"] = s.EstoqueId,
                ["@pid"] = s.ProdutoId,
                ["@qtd"] = s.Quantidade,
                ["@custo"] = s.CustoUnitario,
                ["@cmv"] = s.CmvUnitario,
                ["@origem"] = (int)s.Origem,
                ["@motivo"] = s.Motivo,
                ["@cli"] = s.ClienteId,
                ["@doc"] = s.DocumentoReferencia,
                ["@data"] = s.DataMovimento,
                ["@created_at"] = s.CreatedAt,
                ["@created_by"] = s.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(SaidaProdutoEstoque entity, CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("Movimentos de saída são imutáveis após registro.");

    public Task<IReadOnlyList<SaidaProdutoEstoque>> ListByProdutoAsync(
        Guid produtoId, DateTime? inicio, DateTime? fim, int skip, int take,
        CancellationToken cancellationToken = default)
    {
        var sql = new StringBuilder($@"SELECT {Cols} FROM saida_produto_estoque
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

    private static SaidaProdutoEstoque MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        EstoqueId = r.GetValueOrDefault<Guid>("estoque_id"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        Quantidade = r.GetValueOrDefault<decimal>("quantidade"),
        CustoUnitario = r.GetValueOrDefault<decimal?>("custo_unitario"),
        CmvUnitario = r.GetValueOrDefault<decimal?>("cmv_unitario"),
        Origem = (OrigemMovimento)r.GetValueOrDefault<int>("origem"),
        Motivo = r.GetValueOrDefault<string>("motivo"),
        ClienteId = r.GetValueOrDefault<Guid?>("cliente_id"),
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
