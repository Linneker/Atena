using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class JornadaRepository : BaseRepository<Jornada>, IJornadaRepository
{
    public JornadaRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "jornadas";
    protected override Func<IDataRecord, Jornada> Map => MapEntity;

    public override Task AddAsync(Jornada j, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO jornadas
                (id, tenant_id, nome, tipo, carga_semanal_horas, carga_diaria_horas,
                 janelas_json, permite_marcar_intervalo, tolerancia_minutos, ativo,
                 created_at, created_by)
            VALUES (@id, @t, @nome, @tipo, @csh, @cdh,
                    @janelas, @pmi, @tol, @ativo, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = j.Id,
                ["@t"] = TenantContext.TenantId,
                ["@nome"] = j.Nome,
                ["@tipo"] = j.Tipo.ToString(),
                ["@csh"] = j.CargaSemanalHoras,
                ["@cdh"] = j.CargaDiariaHoras,
                ["@janelas"] = j.JanelasJson,
                ["@pmi"] = j.PermiteMarcarIntervalo ? 1 : 0,
                ["@tol"] = j.ToleranciaMinutos,
                ["@ativo"] = j.Ativo ? 1 : 0,
                ["@createdAt"] = j.CreatedAt,
                ["@createdBy"] = j.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(Jornada j, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE jornadas SET
                nome = @nome, tipo = @tipo, carga_semanal_horas = @csh, carga_diaria_horas = @cdh,
                janelas_json = @janelas, permite_marcar_intervalo = @pmi,
                tolerancia_minutos = @tol, ativo = @ativo,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = j.Id,
                ["@t"] = TenantContext.TenantId,
                ["@nome"] = j.Nome,
                ["@tipo"] = j.Tipo.ToString(),
                ["@csh"] = j.CargaSemanalHoras,
                ["@cdh"] = j.CargaDiariaHoras,
                ["@janelas"] = j.JanelasJson,
                ["@pmi"] = j.PermiteMarcarIntervalo ? 1 : 0,
                ["@tol"] = j.ToleranciaMinutos,
                ["@ativo"] = j.Ativo ? 1 : 0,
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = j.UpdatedBy,
            }, cancellationToken);

    public Task<Jornada?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM jornadas
            WHERE tenant_id = @t AND nome = @nome AND deleted_at IS NULL
            LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId, ["@nome"] = nome },
            cancellationToken);

    private static Jornada MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        Tipo = Enum.TryParse<TipoJornada>(r.GetValueOrDefault<string>("tipo"), out var t) ? t : TipoJornada.Fixa,
        CargaSemanalHoras = r.GetValueOrDefault<decimal>("carga_semanal_horas"),
        CargaDiariaHoras = r.GetValueOrDefault<decimal?>("carga_diaria_horas"),
        JanelasJson = r.GetValueOrDefault<string>("janelas_json") ?? "[]",
        PermiteMarcarIntervalo = r.GetValueOrDefault<int>("permite_marcar_intervalo") == 1,
        ToleranciaMinutos = r.GetValueOrDefault<int>("tolerancia_minutos"),
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
