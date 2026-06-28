using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class MovimentoBancoHorasRepository : BaseRepository<MovimentoBancoHoras>, IMovimentoBancoHorasRepository
{
    public MovimentoBancoHorasRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "movimentos_banco_horas";
    protected override Func<IDataRecord, MovimentoBancoHoras> Map => MapEntity;

    public override Task AddAsync(MovimentoBancoHoras m, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO movimentos_banco_horas
                (id, tenant_id, funcionario_id, data, origem, minutos,
                 referencia_marcacao_id, competencia, observacao, created_at, created_by)
            VALUES (@id, @t, @fid, @d, @origem, @min, @ref, @c, @obs, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = m.Id, ["@t"] = TenantContext.TenantId,
                ["@fid"] = m.FuncionarioId, ["@d"] = m.Data.ToDateTime(TimeOnly.MinValue),
                ["@origem"] = m.Origem.ToString(), ["@min"] = m.Minutos,
                ["@ref"] = m.ReferenciaMarcacaoId, ["@c"] = m.Competencia,
                ["@obs"] = m.Observacao,
                ["@createdAt"] = m.CreatedAt, ["@createdBy"] = m.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(MovimentoBancoHoras m, CancellationToken cancellationToken = default)
        => throw new NotSupportedException("Movimentos de banco de horas são imutáveis (append-only).");

    public Task<IReadOnlyList<MovimentoBancoHoras>> ListByFuncionarioCompetenciaAsync(
        Guid funcionarioId, string competencia, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM movimentos_banco_horas
            WHERE tenant_id = @t AND funcionario_id = @fid AND competencia = @c
              AND deleted_at IS NULL
            ORDER BY data, created_at",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId, ["@fid"] = funcionarioId, ["@c"] = competencia,
            }, cancellationToken);

    private static MovimentoBancoHoras MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FuncionarioId = r.GetValueOrDefault<Guid>("funcionario_id"),
        Data = DateOnly.FromDateTime(r.GetValueOrDefault<DateTime>("data")),
        Origem = Enum.TryParse<OrigemMovimentoBancoHoras>(r.GetValueOrDefault<string>("origem"), out var o) ? o : OrigemMovimentoBancoHoras.Ajuste,
        Minutos = r.GetValueOrDefault<int>("minutos"),
        ReferenciaMarcacaoId = r.GetValueOrDefault<Guid?>("referencia_marcacao_id"),
        Competencia = r.GetValueOrDefault<string>("competencia") ?? string.Empty,
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
