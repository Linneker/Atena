using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Fiscal;

public sealed class NFeRepository : BaseRepository<NFe>, INFeRepository
{
    public NFeRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "nfes";
    protected override Func<IDataRecord, NFe> Map => MapEntity;

    private const string Cols = @"id, tenant_id, numero, serie, chave_acesso, faturamento_id, cliente_id,
        ambiente, modo, data_emissao, data_autorizacao, status,
        protocolo_autorizacao, codigo_status_sefaz, motivo_sefaz,
        valor_total, xml_autorizado_url, xml_enviado_hash,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string ItemCols = @"id, tenant_id, nfe_id, numero_item, produto_id, descricao,
        quantidade, preco_unitario, cfop, ncm,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string EvtCols = @"id, tenant_id, nfe_id, tipo, sequencia, data_evento, descricao,
        protocolo_autorizacao, codigo_status_sefaz, motivo_sefaz, xml_evento_url,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(NFe n, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO nfes
            (id, tenant_id, numero, serie, chave_acesso, faturamento_id, cliente_id,
             ambiente, modo, data_emissao, status, valor_total, xml_enviado_hash,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @num, @ser, @chave, @fat, @cli, @amb, @modo, @em,
             @status, @valor, @hash, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = n.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@num"] = n.Numero,
                ["@ser"] = n.Serie,
                ["@chave"] = n.ChaveAcesso,
                ["@fat"] = n.FaturamentoId,
                ["@cli"] = n.ClienteId,
                ["@amb"] = (int)n.Ambiente,
                ["@modo"] = (int)n.Modo,
                ["@em"] = n.DataEmissao,
                ["@status"] = (int)n.Status,
                ["@valor"] = n.ValorTotal,
                ["@hash"] = n.XmlEnviadoHash,
                ["@created_at"] = n.CreatedAt,
                ["@created_by"] = n.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(NFe entity, CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("Use UpdateStatusAsync para atualizar NF-e.");

    public Task UpdateStatusAsync(Guid id, StatusNFe status, string? codigo, string? motivo, string? protocolo, DateTime? dataAutorizacao, string? chaveAcesso, string? xmlUrl, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE nfes SET
                status = @s,
                codigo_status_sefaz = COALESCE(@cod, codigo_status_sefaz),
                motivo_sefaz = COALESCE(@mot, motivo_sefaz),
                protocolo_autorizacao = COALESCE(@prot, protocolo_autorizacao),
                data_autorizacao = COALESCE(@dataAut, data_autorizacao),
                chave_acesso = COALESCE(@chave, chave_acesso),
                xml_autorizado_url = COALESCE(@url, xml_autorizado_url),
                updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@s"] = (int)status,
                ["@cod"] = codigo,
                ["@mot"] = motivo,
                ["@prot"] = protocolo,
                ["@dataAut"] = dataAutorizacao,
                ["@chave"] = chaveAcesso,
                ["@url"] = xmlUrl,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<IReadOnlyList<NFe>> ListByFiltroAsync(StatusNFe? status, DateTime? inicio, DateTime? fim, int skip, int take, CancellationToken cancellationToken = default)
    {
        var (sql, p) = Filtro(status, inicio, fim);
        sql.Append(" ORDER BY data_emissao DESC LIMIT @take OFFSET @skip");
        p["@take"] = take; p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(StatusNFe? status, DateTime? inicio, DateTime? fim, CancellationToken cancellationToken = default)
    {
        var (sql, p) = Filtro(status, inicio, fim, count: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    private (StringBuilder, Dictionary<string, object?>) Filtro(StatusNFe? status, DateTime? inicio, DateTime? fim, bool count = false)
    {
        var sql = new StringBuilder(count
            ? "SELECT COUNT(*) FROM nfes WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM nfes WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        if (status.HasValue) { sql.Append(" AND status = @s"); p["@s"] = (int)status.Value; }
        if (inicio.HasValue) { sql.Append(" AND data_emissao >= @ini"); p["@ini"] = inicio.Value; }
        if (fim.HasValue) { sql.Append(" AND data_emissao <= @fim"); p["@fim"] = fim.Value; }
        return (sql, p);
    }

    public Task<long> CountAutorizadasNoMesAsync(int ano, int mes, CancellationToken cancellationToken = default)
        => Db.ExecuteScalarAsync<long>(@"
            SELECT COUNT(*) FROM nfes
            WHERE tenant_id = @tenantId AND status = @s AND deleted_at IS NULL
              AND YEAR(data_autorizacao) = @ano AND MONTH(data_autorizacao) = @mes",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@s"] = (int)StatusNFe.Autorizada,
                ["@ano"] = ano,
                ["@mes"] = mes
            }, cancellationToken);

    public Task<IReadOnlyList<NFeItem>> ListItensAsync(Guid nfeId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {ItemCols} FROM nfe_itens WHERE tenant_id = @tenantId AND nfe_id = @nid AND deleted_at IS NULL ORDER BY numero_item",
            MapItem,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@nid"] = nfeId },
            cancellationToken);

    public async Task AddItensAsync(IEnumerable<NFeItem> itens, CancellationToken cancellationToken = default)
    {
        foreach (var i in itens)
        {
            await Db.ExecuteAsync(@"
                INSERT INTO nfe_itens
                (id, tenant_id, nfe_id, numero_item, produto_id, descricao,
                 quantidade, preco_unitario, cfop, ncm, created_at, created_by)
                VALUES (@id, @tenant_id, @nid, @num, @prod, @desc, @qtd, @preco, @cfop, @ncm, @created_at, @created_by)",
                new Dictionary<string, object?>
                {
                    ["@id"] = i.Id,
                    ["@tenant_id"] = TenantContext.TenantId,
                    ["@nid"] = i.NFeId,
                    ["@num"] = i.NumeroItem,
                    ["@prod"] = i.ProdutoId,
                    ["@desc"] = i.Descricao,
                    ["@qtd"] = i.Quantidade,
                    ["@preco"] = i.PrecoUnitario,
                    ["@cfop"] = i.Cfop,
                    ["@ncm"] = i.Ncm,
                    ["@created_at"] = i.CreatedAt,
                    ["@created_by"] = i.CreatedBy
                }, cancellationToken);
        }
    }

    public Task<IReadOnlyList<NFeEvento>> ListEventosAsync(Guid nfeId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {EvtCols} FROM nfe_eventos WHERE tenant_id = @tenantId AND nfe_id = @nid AND deleted_at IS NULL ORDER BY data_evento DESC",
            MapEvento,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@nid"] = nfeId },
            cancellationToken);

    public Task AddEventoAsync(NFeEvento e, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO nfe_eventos
            (id, tenant_id, nfe_id, tipo, sequencia, data_evento, descricao,
             protocolo_autorizacao, codigo_status_sefaz, motivo_sefaz, xml_evento_url,
             created_at, created_by)
            VALUES (@id, @tenant_id, @nid, @tipo, @seq, @data, @desc, @prot, @cod, @mot, @url, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = e.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@nid"] = e.NFeId,
                ["@tipo"] = (int)e.Tipo,
                ["@seq"] = e.Sequencia,
                ["@data"] = e.DataEvento,
                ["@desc"] = e.Descricao,
                ["@prot"] = e.ProtocoloAutorizacao,
                ["@cod"] = e.CodigoStatusSefaz,
                ["@mot"] = e.MotivoSefaz,
                ["@url"] = e.XmlEventoUrl,
                ["@created_at"] = e.CreatedAt,
                ["@created_by"] = e.CreatedBy
            }, cancellationToken);

    private static NFe MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Numero = r.GetValueOrDefault<int>("numero"),
        Serie = r.GetValueOrDefault<int>("serie"),
        ChaveAcesso = r.GetValueOrDefault<string>("chave_acesso"),
        FaturamentoId = r.GetValueOrDefault<Guid?>("faturamento_id"),
        ClienteId = r.GetValueOrDefault<Guid>("cliente_id"),
        Ambiente = (AmbienteFiscal)r.GetValueOrDefault<int>("ambiente"),
        Modo = (ModoTransmissao)r.GetValueOrDefault<int>("modo"),
        DataEmissao = r.GetValueOrDefault<DateTime>("data_emissao"),
        DataAutorizacao = r.GetValueOrDefault<DateTime?>("data_autorizacao"),
        Status = (StatusNFe)r.GetValueOrDefault<int>("status"),
        ProtocoloAutorizacao = r.GetValueOrDefault<string>("protocolo_autorizacao"),
        CodigoStatusSefaz = r.GetValueOrDefault<string>("codigo_status_sefaz"),
        MotivoSefaz = r.GetValueOrDefault<string>("motivo_sefaz"),
        ValorTotal = r.GetValueOrDefault<decimal>("valor_total"),
        XmlAutorizadoUrl = r.GetValueOrDefault<string>("xml_autorizado_url"),
        XmlEnviadoHash = r.GetValueOrDefault<string>("xml_enviado_hash"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static NFeItem MapItem(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        NFeId = r.GetValueOrDefault<Guid>("nfe_id"),
        NumeroItem = r.GetValueOrDefault<int>("numero_item"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        Descricao = r.GetValueOrDefault<string>("descricao") ?? string.Empty,
        Quantidade = r.GetValueOrDefault<decimal>("quantidade"),
        PrecoUnitario = r.GetValueOrDefault<decimal>("preco_unitario"),
        Cfop = r.GetValueOrDefault<string>("cfop"),
        Ncm = r.GetValueOrDefault<string>("ncm"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static NFeEvento MapEvento(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        NFeId = r.GetValueOrDefault<Guid>("nfe_id"),
        Tipo = (TipoEventoNFe)r.GetValueOrDefault<int>("tipo"),
        Sequencia = r.GetValueOrDefault<int>("sequencia"),
        DataEvento = r.GetValueOrDefault<DateTime>("data_evento"),
        Descricao = r.GetValueOrDefault<string>("descricao"),
        ProtocoloAutorizacao = r.GetValueOrDefault<string>("protocolo_autorizacao"),
        CodigoStatusSefaz = r.GetValueOrDefault<string>("codigo_status_sefaz"),
        MotivoSefaz = r.GetValueOrDefault<string>("motivo_sefaz"),
        XmlEventoUrl = r.GetValueOrDefault<string>("xml_evento_url"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
