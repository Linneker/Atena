using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class BancoHorasPoliticaRepository : BaseRepository<BancoHorasPolitica>, IBancoHorasPoliticaRepository
{
    public BancoHorasPoliticaRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "politicas_banco_horas";
    protected override Func<IDataRecord, BancoHorasPolitica> Map => MapEntity;

    public override Task AddAsync(BancoHorasPolitica p, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO politicas_banco_horas
                (id, tenant_id, nome, vigencia_inicio, vigencia_fim, limite_horas_acumular,
                 prazo_compensacao_dias, permite_pagar_excedente, fator_pagamento, ativo,
                 created_at, created_by)
            VALUES (@id, @t, @nome, @vi, @vf, @limite, @prazo, @pagar, @fator, @ativo, @createdAt, @createdBy)",
            BuildParams(p), cancellationToken);

    public override Task UpdateAsync(BancoHorasPolitica p, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE politicas_banco_horas SET
                nome = @nome, vigencia_inicio = @vi, vigencia_fim = @vf,
                limite_horas_acumular = @limite, prazo_compensacao_dias = @prazo,
                permite_pagar_excedente = @pagar, fator_pagamento = @fator, ativo = @ativo,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            BuildParams(p, isUpdate: true), cancellationToken);

    public Task<BancoHorasPolitica?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM politicas_banco_horas
            WHERE tenant_id = @t AND nome = @n AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId, ["@n"] = nome },
            cancellationToken);

    private Dictionary<string, object?> BuildParams(BancoHorasPolitica p, bool isUpdate = false)
    {
        var d = new Dictionary<string, object?>
        {
            ["@id"] = p.Id,
            ["@t"] = TenantContext.TenantId,
            ["@nome"] = p.Nome,
            ["@vi"] = p.VigenciaInicio.ToDateTime(TimeOnly.MinValue),
            ["@vf"] = p.VigenciaFim?.ToDateTime(TimeOnly.MinValue),
            ["@limite"] = p.LimiteHorasAcumular,
            ["@prazo"] = p.PrazoCompensacaoDias,
            ["@pagar"] = p.PermitePagarExcedente ? 1 : 0,
            ["@fator"] = p.FatorPagamento,
            ["@ativo"] = p.Ativo ? 1 : 0,
        };
        if (isUpdate)
        {
            d["@updatedAt"] = DateTime.UtcNow;
            d["@updatedBy"] = p.UpdatedBy;
        }
        else
        {
            d["@createdAt"] = p.CreatedAt;
            d["@createdBy"] = p.CreatedBy;
        }
        return d;
    }

    private static BancoHorasPolitica MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        VigenciaInicio = DateOnly.FromDateTime(r.GetValueOrDefault<DateTime>("vigencia_inicio")),
        VigenciaFim = r.GetValueOrDefault<DateTime?>("vigencia_fim") is { } vf ? DateOnly.FromDateTime(vf) : null,
        LimiteHorasAcumular = r.GetValueOrDefault<decimal>("limite_horas_acumular"),
        PrazoCompensacaoDias = r.GetValueOrDefault<int>("prazo_compensacao_dias"),
        PermitePagarExcedente = r.GetValueOrDefault<int>("permite_pagar_excedente") == 1,
        FatorPagamento = r.GetValueOrDefault<decimal>("fator_pagamento"),
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
