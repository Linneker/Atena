using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Cria a tabela de referência <c>ufs</c> (catálogo nacional, não tenant-scoped) e semeia
/// as 27 unidades federativas com sigla, nome e código IBGE. Dado canônico e estável —
/// embutido inline na migration (não depende de arquivo externo).
/// </summary>
public sealed class V20260514001_AddTabelaUfs : IMigration
{
    public long Version => 20260514001;
    public string Name => "AddTabelaUfs";

    private static readonly (string Sigla, string Nome, int CodigoIbge)[] Ufs =
    {
        ("RO", "Rondônia", 11), ("AC", "Acre", 12), ("AM", "Amazonas", 13),
        ("RR", "Roraima", 14), ("PA", "Pará", 15), ("AP", "Amapá", 16),
        ("TO", "Tocantins", 17), ("MA", "Maranhão", 21), ("PI", "Piauí", 22),
        ("CE", "Ceará", 23), ("RN", "Rio Grande do Norte", 24), ("PB", "Paraíba", 25),
        ("PE", "Pernambuco", 26), ("AL", "Alagoas", 27), ("SE", "Sergipe", 28),
        ("BA", "Bahia", 29), ("MG", "Minas Gerais", 31), ("ES", "Espírito Santo", 32),
        ("RJ", "Rio de Janeiro", 33), ("SP", "São Paulo", 35), ("PR", "Paraná", 41),
        ("SC", "Santa Catarina", 42), ("RS", "Rio Grande do Sul", 43),
        ("MS", "Mato Grosso do Sul", 50), ("MT", "Mato Grosso", 51),
        ("GO", "Goiás", 52), ("DF", "Distrito Federal", 53),
    };

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS ufs (
                sigla CHAR(2) NOT NULL PRIMARY KEY,
                nome VARCHAR(60) NOT NULL,
                codigo_ibge INT NOT NULL,
                UNIQUE KEY uq_ufs_codigo_ibge (codigo_ibge)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");

        foreach (var (sigla, nome, codigo) in Ufs)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"INSERT INTO ufs (sigla, nome, codigo_ibge)
                                VALUES (@sigla, @nome, @codigo)
                                ON DUPLICATE KEY UPDATE nome = VALUES(nome), codigo_ibge = VALUES(codigo_ibge);";
            AddParam(cmd, "@sigla", sigla);
            AddParam(cmd, "@nome", nome);
            AddParam(cmd, "@codigo", codigo);
            cmd.ExecuteNonQuery();
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => Exec(connection, transaction, "DROP TABLE IF EXISTS ufs;");

    private static void Exec(IDbConnection conn, IDbTransaction tx, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    private static void AddParam(IDbCommand cmd, string name, object value)
    {
        var p = cmd.CreateParameter();
        p.ParameterName = name;
        p.Value = value;
        cmd.Parameters.Add(p);
    }
}
