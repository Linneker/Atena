using System.Data;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using MySqlConnector;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh.Oficial671;

/// <summary>
/// Implementação atômica de <see cref="INumeradorNsr"/> sobre MySQL.
/// Idiom <c>INSERT … ON DUPLICATE KEY UPDATE col = LAST_INSERT_ID(col + 1)</c> — uma
/// única ida ao banco, sem gap entre lock e write, impossível duas sessões obterem o
/// mesmo NSR. Cópia adaptada do <c>NumeradorNFe</c>.
/// </summary>
public sealed class NumeradorNsr : INumeradorNsr
{
    private readonly IDataConfiguration _db;
    private readonly ITenantContext _tenant;

    public NumeradorNsr(IDataConfiguration db, ITenantContext tenant)
    {
        _db = db;
        _tenant = tenant;
    }

    public async Task<long> ProximoAsync(Guid empresaId, CancellationToken cancellationToken = default)
    {
        if (empresaId == Guid.Empty) throw new ArgumentException("empresaId obrigatório.", nameof(empresaId));
        var tenantId = _tenant.TenantId;
        if (tenantId == Guid.Empty)
            throw new InvalidOperationException("TenantContext não autenticado — não é possível reservar NSR.");

        await using var conn = (MySqlConnection)_db.CreateConnection();
        await conn.OpenAsync(cancellationToken);

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO numerador_nsr (id, tenant_id, empresa_id, ultimo_numero, atualizado_em)
            VALUES (@id, @tenantId, @empresaId, LAST_INSERT_ID(1), @agora)
            ON DUPLICATE KEY UPDATE
                ultimo_numero = LAST_INSERT_ID(ultimo_numero + 1),
                atualizado_em = @agora;
            SELECT LAST_INSERT_ID();";
        cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
        cmd.Parameters.AddWithValue("@tenantId", tenantId.ToString());
        cmd.Parameters.AddWithValue("@empresaId", empresaId.ToString());
        cmd.Parameters.AddWithValue("@agora", DateTime.UtcNow);

        var raw = await cmd.ExecuteScalarAsync(cancellationToken)
            ?? throw new InvalidOperationException("Falha ao reservar NSR — LAST_INSERT_ID() retornou nulo.");
        return Convert.ToInt64(raw);
    }

    public async Task<long> UltimoAsync(Guid empresaId, CancellationToken cancellationToken = default)
    {
        var tenantId = _tenant.TenantId;
        await using var conn = (MySqlConnection)_db.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT COALESCE(ultimo_numero, 0)
            FROM numerador_nsr
            WHERE tenant_id = @t AND empresa_id = @e;";
        cmd.Parameters.AddWithValue("@t", tenantId.ToString());
        cmd.Parameters.AddWithValue("@e", empresaId.ToString());
        var raw = await cmd.ExecuteScalarAsync(cancellationToken);
        return raw is null or DBNull ? 0L : Convert.ToInt64(raw);
    }
}
