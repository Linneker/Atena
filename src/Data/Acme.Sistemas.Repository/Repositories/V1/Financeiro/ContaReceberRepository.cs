using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class ContaReceberRepository : BaseRepository<ContaReceber>, IContaReceberRepository
{
    public ContaReceberRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "contas_receber";
    protected override Func<IDataRecord, ContaReceber> Map => MapEntity;

    private const string Cols = @"id, tenant_id, descricao, cliente_id, receita_id, plano_de_contas_id,
        valor_original, valor_recebido, data_vencimento, data_recebimento, status, observacao_recebimento,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(ContaReceber c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO contas_receber
            (id, tenant_id, descricao, cliente_id, receita_id, plano_de_contas_id,
             valor_original, valor_recebido, data_vencimento, data_recebimento, status, observacao_recebimento,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @descricao, @cliente_id, @receita_id, @plano_de_contas_id,
             @valor_original, @valor_recebido, @data_vencimento, @data_recebimento, @status, @obs,
             @created_at, @created_by)",
            BuildParams(c, isInsert: true), cancellationToken);

    public override Task UpdateAsync(ContaReceber c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE contas_receber SET
                descricao = @descricao, cliente_id = @cliente_id,
                receita_id = @receita_id, plano_de_contas_id = @plano_de_contas_id,
                valor_original = @valor_original,
                data_vencimento = @data_vencimento,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            BuildParams(c, isInsert: false), cancellationToken);

    public Task ReceberAsync(ContaReceber c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE contas_receber SET
                valor_recebido = @valor_recebido,
                data_recebimento = @data_recebimento,
                status = @status,
                observacao_recebimento = @obs,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = c.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@valor_recebido"] = c.ValorRecebido,
                ["@data_recebimento"] = c.DataRecebimento,
                ["@status"] = (int)c.Status,
                ["@obs"] = c.ObservacaoRecebimento,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = c.UpdatedBy
            }, cancellationToken);

    public Task<IReadOnlyList<ContaReceber>> ListByFiltroAsync(
        StatusConta? status, DateTime? vencimentoInicio, DateTime? vencimentoFim,
        Guid? clienteId, int? diasAtrasoMinimo, int skip, int take,
        CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(status, vencimentoInicio, vencimentoFim, clienteId, diasAtrasoMinimo);
        sql.Append(" ORDER BY data_vencimento ASC LIMIT @take OFFSET @skip");
        p["@take"] = take;
        p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(
        StatusConta? status, DateTime? vencimentoInicio, DateTime? vencimentoFim,
        Guid? clienteId, int? diasAtrasoMinimo,
        CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(status, vencimentoInicio, vencimentoFim, clienteId, diasAtrasoMinimo, countOnly: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    private (StringBuilder, Dictionary<string, object?>) BuildFiltro(
        StatusConta? status, DateTime? vencimentoInicio, DateTime? vencimentoFim,
        Guid? clienteId, int? diasAtrasoMinimo, bool countOnly = false)
    {
        var sql = new StringBuilder(countOnly
            ? "SELECT COUNT(*) FROM contas_receber WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM contas_receber WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };

        if (status.HasValue) { sql.Append(" AND status = @status"); p["@status"] = (int)status.Value; }
        if (vencimentoInicio.HasValue) { sql.Append(" AND data_vencimento >= @inicio"); p["@inicio"] = vencimentoInicio.Value; }
        if (vencimentoFim.HasValue) { sql.Append(" AND data_vencimento <= @fim"); p["@fim"] = vencimentoFim.Value; }
        if (clienteId.HasValue) { sql.Append(" AND cliente_id = @clienteId"); p["@clienteId"] = clienteId.Value; }
        if (diasAtrasoMinimo.HasValue)
        {
            sql.Append(" AND status IN (@sP, @sPP) AND DATEDIFF(CURDATE(), data_vencimento) >= @atraso");
            p["@atraso"] = diasAtrasoMinimo.Value;
            p["@sP"] = (int)StatusConta.Pendente;
            p["@sPP"] = (int)StatusConta.PagoParcial;
        }

        return (sql, p);
    }

    private Dictionary<string, object?> BuildParams(ContaReceber c, bool isInsert)
    {
        var p = new Dictionary<string, object?>
        {
            ["@id"] = c.Id,
            ["@descricao"] = c.Descricao,
            ["@cliente_id"] = c.ClienteId,
            ["@receita_id"] = c.ReceitaId,
            ["@plano_de_contas_id"] = c.PlanoDeContasId,
            ["@valor_original"] = c.ValorOriginal,
            ["@valor_recebido"] = c.ValorRecebido,
            ["@data_vencimento"] = c.DataVencimento,
            ["@data_recebimento"] = c.DataRecebimento,
            ["@status"] = (int)c.Status,
            ["@obs"] = c.ObservacaoRecebimento
        };
        if (isInsert)
        {
            p["@tenant_id"] = TenantContext.TenantId;
            p["@created_at"] = c.CreatedAt;
            p["@created_by"] = c.CreatedBy;
        }
        else
        {
            p["@tenantId"] = TenantContext.TenantId;
            p["@updated_at"] = DateTime.UtcNow;
            p["@updated_by"] = c.UpdatedBy;
        }
        return p;
    }

    private static ContaReceber MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Descricao = r.GetValueOrDefault<string>("descricao") ?? string.Empty,
        ClienteId = r.GetValueOrDefault<Guid?>("cliente_id"),
        ReceitaId = r.GetValueOrDefault<Guid?>("receita_id"),
        PlanoDeContasId = r.GetValueOrDefault<Guid?>("plano_de_contas_id"),
        ValorOriginal = r.GetValueOrDefault<decimal>("valor_original"),
        ValorRecebido = r.GetValueOrDefault<decimal>("valor_recebido"),
        DataVencimento = r.GetValueOrDefault<DateTime>("data_vencimento"),
        DataRecebimento = r.GetValueOrDefault<DateTime?>("data_recebimento"),
        Status = (StatusConta)r.GetValueOrDefault<int>("status"),
        ObservacaoRecebimento = r.GetValueOrDefault<string>("observacao_recebimento"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
