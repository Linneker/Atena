using System.Data;
using Acme.Sistemas.Domain.Interfaces.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using MySqlConnector;

namespace Acme.Sistemas.Repository.Repositories.V1.Fiscal;

/// <summary>
/// Implementação atômica de <see cref="INumeradorNFe"/> sobre MySQL usando o idiom
/// <c>INSERT ... ON DUPLICATE KEY UPDATE col = LAST_INSERT_ID(col + 1)</c>, que:
/// 1) cria a linha se não existir (com ultimo_numero=1);
/// 2) incrementa atomicamente se existir;
/// 3) expõe o valor novo via <c>LAST_INSERT_ID()</c> da mesma sessão.
///
/// Vantagens vs <c>SELECT … FOR UPDATE</c> + UPDATE:
/// - Uma única ida ao banco (não dois round-trips).
/// - Sem gap entre lock e write — impossível de duas sessões obterem o mesmo número.
/// - Funciona em InnoDB sem precisar elevar isolation level.
///
/// Garantia: chamadas concorrentes recebem números distintos consecutivos; pulos só
/// acontecem se a transação falhar (e nesse caso o número foi "consumido" e deve ser
/// inutilizado via NFeInutilizacao4).
/// </summary>
public sealed class NumeradorNFe : INumeradorNFe
{
    private readonly IDataConfiguration _db;
    private readonly ITenantContext _tenant;

    public NumeradorNFe(IDataConfiguration db, ITenantContext tenant)
    {
        _db = db;
        _tenant = tenant;
    }

    public async Task<long> ProximoAsync(string cnpj, int serie, CancellationToken cancellationToken = default)
    {
        if (cnpj.Length != 14) throw new ArgumentException("CNPJ deve ter 14 dígitos.", nameof(cnpj));
        if (serie < 0) throw new ArgumentException("Série não pode ser negativa.", nameof(serie));

        var tenantId = _tenant.TenantId;
        if (tenantId == Guid.Empty)
            throw new InvalidOperationException("TenantContext não autenticado — não é possível reservar número.");

        // Uma única sessão para garantir que LAST_INSERT_ID() reflita o INSERT acima.
        await using var conn = (MySqlConnection)_db.CreateConnection();
        await conn.OpenAsync(cancellationToken);

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO nfe_numeracao (id, tenant_id, cnpj, serie, ultimo_numero, atualizado_em)
            VALUES (@id, @tenantId, @cnpj, @serie, LAST_INSERT_ID(1), @agora)
            ON DUPLICATE KEY UPDATE
                ultimo_numero = LAST_INSERT_ID(ultimo_numero + 1),
                atualizado_em = @agora;
            SELECT LAST_INSERT_ID();";
        cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
        cmd.Parameters.AddWithValue("@tenantId", tenantId.ToString());
        cmd.Parameters.AddWithValue("@cnpj", cnpj);
        cmd.Parameters.AddWithValue("@serie", serie);
        cmd.Parameters.AddWithValue("@agora", DateTime.UtcNow);

        var raw = await cmd.ExecuteScalarAsync(cancellationToken)
            ?? throw new InvalidOperationException("Falha ao reservar número — LAST_INSERT_ID() retornou nulo.");
        return Convert.ToInt64(raw);
    }

    public async Task AjustarUltimoNumeroAsync(string cnpj, int serie, long ultimoNumero, CancellationToken cancellationToken = default)
    {
        if (ultimoNumero < 0) throw new ArgumentException("ultimoNumero não pode ser negativo.", nameof(ultimoNumero));

        var tenantId = _tenant.TenantId;
        await _db.ExecuteAsync(@"
            INSERT INTO nfe_numeracao (id, tenant_id, cnpj, serie, ultimo_numero, atualizado_em)
            VALUES (@id, @tenantId, @cnpj, @serie, @num, @agora)
            ON DUPLICATE KEY UPDATE
                ultimo_numero = @num,
                atualizado_em = @agora;",
            new Dictionary<string, object?>
            {
                ["@id"] = Guid.NewGuid().ToString(),
                ["@tenantId"] = tenantId.ToString(),
                ["@cnpj"] = cnpj,
                ["@serie"] = serie,
                ["@num"] = ultimoNumero,
                ["@agora"] = DateTime.UtcNow,
            }, cancellationToken);
    }
}
