using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class BancoHorasSaldoRepository : BaseRepository<BancoHorasSaldo>, IBancoHorasSaldoRepository
{
    public BancoHorasSaldoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "banco_horas_saldo";
    protected override Func<IDataRecord, BancoHorasSaldo> Map => MapEntity;

    public override Task AddAsync(BancoHorasSaldo s, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO banco_horas_saldo
                (id, tenant_id, funcionario_id, competencia, horas_devidas, horas_realizadas,
                 saldo_minutos, politica_id, created_at, created_by)
            VALUES (@id, @t, @fid, @c, @hd, @hr, @sm, @pid, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = s.Id, ["@t"] = TenantContext.TenantId,
                ["@fid"] = s.FuncionarioId, ["@c"] = s.Competencia,
                ["@hd"] = s.HorasDevidas, ["@hr"] = s.HorasRealizadas,
                ["@sm"] = s.SaldoMinutos, ["@pid"] = s.PoliticaId,
                ["@createdAt"] = s.CreatedAt, ["@createdBy"] = s.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(BancoHorasSaldo s, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE banco_horas_saldo SET
                horas_devidas = @hd, horas_realizadas = @hr, saldo_minutos = @sm,
                politica_id = @pid, updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t",
            new Dictionary<string, object?>
            {
                ["@id"] = s.Id, ["@t"] = TenantContext.TenantId,
                ["@hd"] = s.HorasDevidas, ["@hr"] = s.HorasRealizadas,
                ["@sm"] = s.SaldoMinutos, ["@pid"] = s.PoliticaId,
                ["@updatedAt"] = DateTime.UtcNow, ["@updatedBy"] = s.UpdatedBy,
            }, cancellationToken);

    public Task<BancoHorasSaldo?> GetByFuncionarioCompetenciaAsync(
        Guid funcionarioId, string competencia, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM banco_horas_saldo
            WHERE tenant_id = @t AND funcionario_id = @fid AND competencia = @c LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId, ["@fid"] = funcionarioId, ["@c"] = competencia,
            }, cancellationToken);

    private static BancoHorasSaldo MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FuncionarioId = r.GetValueOrDefault<Guid>("funcionario_id"),
        Competencia = r.GetValueOrDefault<string>("competencia") ?? string.Empty,
        HorasDevidas = r.GetValueOrDefault<decimal>("horas_devidas"),
        HorasRealizadas = r.GetValueOrDefault<decimal>("horas_realizadas"),
        SaldoMinutos = r.GetValueOrDefault<int>("saldo_minutos"),
        PoliticaId = r.GetValueOrDefault<Guid?>("politica_id"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
    };
}
