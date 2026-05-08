using System.Data;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class ConciliacaoBancariaRepository : BaseRepository<ConciliacaoBancaria>, IConciliacaoBancariaRepository
{
    public ConciliacaoBancariaRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "conciliacoes_bancarias";
    protected override Func<IDataRecord, ConciliacaoBancaria> Map => MapConciliacao;

    private const string CCols = @"id, tenant_id, banco, agencia, conta, periodo_inicio, periodo_fim,
        formato_arquivo, status, total_lancamentos, total_conciliados,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string ICols = @"id, tenant_id, conciliacao_id, data_movimento, valor, tipo,
        descricao, documento_bancario, status, conta_pagar_id, conta_receber_id,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(ConciliacaoBancaria c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO conciliacoes_bancarias
            (id, tenant_id, banco, agencia, conta, periodo_inicio, periodo_fim, formato_arquivo,
             status, total_lancamentos, total_conciliados, created_at, created_by)
            VALUES
            (@id, @tenant_id, @banco, @agencia, @conta, @inicio, @fim, @formato,
             @status, @total, @conc, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = c.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@banco"] = c.Banco,
                ["@agencia"] = c.Agencia,
                ["@conta"] = c.Conta,
                ["@inicio"] = c.PeriodoInicio,
                ["@fim"] = c.PeriodoFim,
                ["@formato"] = c.FormatoArquivo,
                ["@status"] = (int)c.Status,
                ["@total"] = c.TotalLancamentos,
                ["@conc"] = c.TotalConciliados,
                ["@created_at"] = c.CreatedAt,
                ["@created_by"] = c.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(ConciliacaoBancaria c, CancellationToken cancellationToken = default)
        => UpdateTotaisAsync(c.Id, c.TotalLancamentos, c.TotalConciliados, c.Status, cancellationToken);

    public Task UpdateTotaisAsync(Guid conciliacaoId, int totalLancamentos, int totalConciliados, StatusConciliacao status, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE conciliacoes_bancarias SET
                total_lancamentos = @total,
                total_conciliados = @conc,
                status = @status,
                updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = conciliacaoId,
                ["@tenantId"] = TenantContext.TenantId,
                ["@total"] = totalLancamentos,
                ["@conc"] = totalConciliados,
                ["@status"] = (int)status,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public async Task AddItensAsync(IEnumerable<ItemExtrato> itens, CancellationToken cancellationToken = default)
    {
        foreach (var i in itens)
        {
            await Db.ExecuteAsync(@"
                INSERT INTO itens_extrato
                (id, tenant_id, conciliacao_id, data_movimento, valor, tipo,
                 descricao, documento_bancario, status, conta_pagar_id, conta_receber_id,
                 created_at, created_by)
                VALUES
                (@id, @tenant_id, @conc, @data, @valor, @tipo, @descricao, @doc,
                 @status, @cp, @cr, @created_at, @created_by)",
                new Dictionary<string, object?>
                {
                    ["@id"] = i.Id,
                    ["@tenant_id"] = TenantContext.TenantId,
                    ["@conc"] = i.ConciliacaoId,
                    ["@data"] = i.DataMovimento,
                    ["@valor"] = i.Valor,
                    ["@tipo"] = (int)i.Tipo,
                    ["@descricao"] = i.Descricao,
                    ["@doc"] = i.DocumentoBancario,
                    ["@status"] = (int)i.Status,
                    ["@cp"] = i.ContaPagarId,
                    ["@cr"] = i.ContaReceberId,
                    ["@created_at"] = i.CreatedAt,
                    ["@created_by"] = i.CreatedBy
                }, cancellationToken);
        }
    }

    public Task UpdateItemAsync(ItemExtrato i, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE itens_extrato SET
                status = @status,
                conta_pagar_id = @cp,
                conta_receber_id = @cr,
                updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = i.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@status"] = (int)i.Status,
                ["@cp"] = i.ContaPagarId,
                ["@cr"] = i.ContaReceberId,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<IReadOnlyList<ItemExtrato>> ListItensAsync(Guid conciliacaoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {ICols} FROM itens_extrato WHERE tenant_id = @tenantId AND conciliacao_id = @id AND deleted_at IS NULL ORDER BY data_movimento",
            MapItem,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@id"] = conciliacaoId },
            cancellationToken);

    private static ConciliacaoBancaria MapConciliacao(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Banco = r.GetValueOrDefault<string>("banco") ?? string.Empty,
        Agencia = r.GetValueOrDefault<string>("agencia"),
        Conta = r.GetValueOrDefault<string>("conta"),
        PeriodoInicio = r.GetValueOrDefault<DateTime>("periodo_inicio"),
        PeriodoFim = r.GetValueOrDefault<DateTime>("periodo_fim"),
        FormatoArquivo = r.GetValueOrDefault<string>("formato_arquivo") ?? "CSV",
        Status = (StatusConciliacao)r.GetValueOrDefault<int>("status"),
        TotalLancamentos = r.GetValueOrDefault<int>("total_lancamentos"),
        TotalConciliados = r.GetValueOrDefault<int>("total_conciliados"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static ItemExtrato MapItem(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        ConciliacaoId = r.GetValueOrDefault<Guid>("conciliacao_id"),
        DataMovimento = r.GetValueOrDefault<DateTime>("data_movimento"),
        Valor = r.GetValueOrDefault<decimal>("valor"),
        Tipo = (TipoMovimentoExtrato)r.GetValueOrDefault<int>("tipo"),
        Descricao = r.GetValueOrDefault<string>("descricao"),
        DocumentoBancario = r.GetValueOrDefault<string>("documento_bancario"),
        Status = (StatusItemExtrato)r.GetValueOrDefault<int>("status"),
        ContaPagarId = r.GetValueOrDefault<Guid?>("conta_pagar_id"),
        ContaReceberId = r.GetValueOrDefault<Guid?>("conta_receber_id"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
