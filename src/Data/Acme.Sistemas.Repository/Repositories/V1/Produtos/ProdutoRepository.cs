using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Produtos;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Produtos;

public sealed class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
{
    public ProdutoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "produtos";
    protected override Func<IDataRecord, Produto> Map => MapEntity;

    private const string Cols = @"id, tenant_id, codigo, nome, descricao, codigo_barras,
        unidade_medida, tipo_produto_id, fornecedor_id, custo_medio, estoque_minimo, status,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string ValorCols = @"id, tenant_id, produto_id, tipo_valor_produto_id, valor,
        vigencia_inicio, vigencia_fim,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(Produto p, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO produtos
            (id, tenant_id, codigo, nome, descricao, codigo_barras, unidade_medida,
             tipo_produto_id, fornecedor_id, custo_medio, estoque_minimo, status,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @codigo, @nome, @descricao, @cb, @um,
             @tp, @forn, @custo, @em, @status, @created_at, @created_by)",
            BuildParams(p, isInsert: true), cancellationToken);

    public override Task UpdateAsync(Produto p, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE produtos SET
                codigo = @codigo, nome = @nome, descricao = @descricao,
                codigo_barras = @cb, unidade_medida = @um,
                tipo_produto_id = @tp, fornecedor_id = @forn,
                custo_medio = @custo, estoque_minimo = @em, status = @status,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            BuildParams(p, isInsert: false), cancellationToken);

    public Task<Produto?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM produtos WHERE tenant_id = @tenantId AND codigo = @codigo AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@codigo"] = codigo },
            cancellationToken);

    public Task<Produto?> GetByCodigoBarrasAsync(string codigoBarras, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM produtos WHERE tenant_id = @tenantId AND codigo_barras = @cb AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@cb"] = codigoBarras },
            cancellationToken);

    public Task<IReadOnlyList<Produto>> ListByFiltroAsync(string? termo, int skip, int take, CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(termo);
        sql.Append(" ORDER BY nome LIMIT @take OFFSET @skip");
        p["@take"] = take;
        p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(string? termo, CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(termo, countOnly: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    public Task<IReadOnlyList<ValorProduto>> ListPrecosAsync(Guid produtoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $@"SELECT {ValorCols} FROM valores_produto
               WHERE tenant_id = @tenantId AND produto_id = @id AND deleted_at IS NULL
               ORDER BY vigencia_inicio DESC",
            MapValor,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@id"] = produtoId },
            cancellationToken);

    public Task UpsertPrecoAsync(ValorProduto preco, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO valores_produto
            (id, tenant_id, produto_id, tipo_valor_produto_id, valor,
             vigencia_inicio, vigencia_fim, created_at, created_by)
            VALUES
            (@id, @tenant_id, @produto_id, @tvpId, @valor,
             @inicio, @fim, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = preco.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@produto_id"] = preco.ProdutoId,
                ["@tvpId"] = preco.TipoValorProdutoId,
                ["@valor"] = preco.Valor,
                ["@inicio"] = preco.VigenciaInicio,
                ["@fim"] = preco.VigenciaFim,
                ["@created_at"] = preco.CreatedAt,
                ["@created_by"] = preco.CreatedBy
            }, cancellationToken);

    public Task ExpirarPrecosAtuaisAsync(Guid produtoId, Guid tipoValorProdutoId, DateTime dataFim, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE valores_produto SET vigencia_fim = @fim, updated_at = @updated_at
            WHERE tenant_id = @tenantId AND produto_id = @pid AND tipo_valor_produto_id = @tvpId
              AND vigencia_fim IS NULL AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@pid"] = produtoId,
                ["@tvpId"] = tipoValorProdutoId,
                ["@fim"] = dataFim,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    private (StringBuilder, Dictionary<string, object?>) BuildFiltro(string? termo, bool countOnly = false)
    {
        var sql = new StringBuilder(countOnly
            ? "SELECT COUNT(*) FROM produtos WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM produtos WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        if (!string.IsNullOrWhiteSpace(termo))
        {
            sql.Append(" AND (nome LIKE @t OR codigo LIKE @t OR codigo_barras LIKE @t)");
            p["@t"] = $"%{termo}%";
        }
        return (sql, p);
    }

    private Dictionary<string, object?> BuildParams(Produto p, bool isInsert)
    {
        var d = new Dictionary<string, object?>
        {
            ["@id"] = p.Id,
            ["@codigo"] = p.Codigo,
            ["@nome"] = p.Nome,
            ["@descricao"] = p.Descricao,
            ["@cb"] = p.CodigoBarras,
            ["@um"] = p.UnidadeMedida,
            ["@tp"] = p.TipoProdutoId,
            ["@forn"] = p.FornecedorId,
            ["@custo"] = p.CustoMedio,
            ["@em"] = p.EstoqueMinimo,
            ["@status"] = (int)p.Status
        };
        if (isInsert)
        {
            d["@tenant_id"] = TenantContext.TenantId;
            d["@created_at"] = p.CreatedAt;
            d["@created_by"] = p.CreatedBy;
        }
        else
        {
            d["@tenantId"] = TenantContext.TenantId;
            d["@updated_at"] = DateTime.UtcNow;
            d["@updated_by"] = p.UpdatedBy;
        }
        return d;
    }

    private static Produto MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Codigo = r.GetValueOrDefault<string>("codigo") ?? string.Empty,
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        Descricao = r.GetValueOrDefault<string>("descricao"),
        CodigoBarras = r.GetValueOrDefault<string>("codigo_barras"),
        UnidadeMedida = r.GetValueOrDefault<string>("unidade_medida") ?? "UN",
        TipoProdutoId = r.GetValueOrDefault<Guid?>("tipo_produto_id"),
        FornecedorId = r.GetValueOrDefault<Guid?>("fornecedor_id"),
        CustoMedio = r.GetValueOrDefault<decimal?>("custo_medio"),
        EstoqueMinimo = r.GetValueOrDefault<decimal?>("estoque_minimo"),
        Status = (StatusAtivo)r.GetValueOrDefault<int>("status"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static ValorProduto MapValor(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        TipoValorProdutoId = r.GetValueOrDefault<Guid>("tipo_valor_produto_id"),
        Valor = r.GetValueOrDefault<decimal>("valor"),
        VigenciaInicio = r.GetValueOrDefault<DateTime>("vigencia_inicio"),
        VigenciaFim = r.GetValueOrDefault<DateTime?>("vigencia_fim"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
