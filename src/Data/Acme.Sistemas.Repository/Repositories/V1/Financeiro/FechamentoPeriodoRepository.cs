using System.Data;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class FechamentoPeriodoRepository : BaseRepository<FechamentoPeriodo>, IFechamentoPeriodoRepository
{
    public FechamentoPeriodoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "fechamento_periodos";
    protected override Func<IDataRecord, FechamentoPeriodo> Map => MapEntity;

    private const string Cols = @"id, tenant_id, ano, mes, fechado_em, fechado_por,
        total_receitas, total_despesas, resultado, observacao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(FechamentoPeriodo f, CancellationToken cancellationToken = default)
    {
        return Db.ExecuteAsync(@"
            INSERT INTO fechamento_periodos
            (id, tenant_id, ano, mes, fechado_em, fechado_por,
             total_receitas, total_despesas, resultado, observacao,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @ano, @mes, @fechado_em, @fechado_por,
             @total_receitas, @total_despesas, @resultado, @observacao,
             @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = f.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@ano"] = f.Ano,
                ["@mes"] = f.Mes,
                ["@fechado_em"] = f.FechadoEm,
                ["@fechado_por"] = f.FechadoPor,
                ["@total_receitas"] = f.TotalReceitas,
                ["@total_despesas"] = f.TotalDespesas,
                ["@resultado"] = f.Resultado,
                ["@observacao"] = f.Observacao,
                ["@created_at"] = f.CreatedAt,
                ["@created_by"] = f.CreatedBy
            }, cancellationToken);
    }

    public override Task UpdateAsync(FechamentoPeriodo f, CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("Fechamento de período é imutável.");

    public Task<FechamentoPeriodo?> GetByPeriodoAsync(int ano, int mes, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM fechamento_periodos WHERE tenant_id = @tenantId AND ano = @ano AND mes = @mes AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@ano"] = ano,
                ["@mes"] = mes
            }, cancellationToken);

    private static FechamentoPeriodo MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Ano = r.GetValueOrDefault<int>("ano"),
        Mes = r.GetValueOrDefault<int>("mes"),
        FechadoEm = r.GetValueOrDefault<DateTime>("fechado_em"),
        FechadoPor = r.GetValueOrDefault<Guid?>("fechado_por"),
        TotalReceitas = r.GetValueOrDefault<decimal>("total_receitas"),
        TotalDespesas = r.GetValueOrDefault<decimal>("total_despesas"),
        Resultado = r.GetValueOrDefault<decimal>("resultado"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
