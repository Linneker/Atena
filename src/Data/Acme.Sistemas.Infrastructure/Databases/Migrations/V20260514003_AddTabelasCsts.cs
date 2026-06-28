using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Cria as 4 tabelas de CST (csts_icms, csts_pis, csts_cofins, csts_ipi) e semeia as listas
/// oficiais. Dado canônico e estável — inline. PIS e COFINS compartilham a mesma tabela de
/// situação tributária (mas mantidas separadas conforme contrato do change).
/// </summary>
public sealed class V20260514003_AddTabelasCsts : IMigration
{
    public long Version => 20260514003;
    public string Name => "AddTabelasCsts";

    private static readonly (string Codigo, string Descricao)[] Icms =
    {
        ("00", "Tributada integralmente"),
        ("10", "Tributada e com cobrança do ICMS por substituição tributária"),
        ("20", "Com redução de base de cálculo"),
        ("30", "Isenta/não tributada e com cobrança do ICMS por substituição tributária"),
        ("40", "Isenta"),
        ("41", "Não tributada"),
        ("50", "Suspensão"),
        ("51", "Diferimento"),
        ("60", "ICMS cobrado anteriormente por substituição tributária"),
        ("70", "Com redução de base de cálculo e cobrança do ICMS por ST"),
        ("90", "Outras"),
    };

    private static readonly (string Codigo, string Descricao)[] PisCofins =
    {
        ("01", "Operação tributável com alíquota básica"),
        ("02", "Operação tributável com alíquota diferenciada"),
        ("03", "Operação tributável com alíquota por unidade de medida de produto"),
        ("04", "Operação tributável monofásica — revenda a alíquota zero"),
        ("05", "Operação tributável por substituição tributária"),
        ("06", "Operação tributável a alíquota zero"),
        ("07", "Operação isenta da contribuição"),
        ("08", "Operação sem incidência da contribuição"),
        ("09", "Operação com suspensão da contribuição"),
        ("49", "Outras operações de saída"),
        ("50", "Operação com direito a crédito — vinculada exclusivamente a receita tributada no mercado interno"),
        ("70", "Operação de aquisição sem direito a crédito"),
        ("98", "Outras operações de entrada"),
        ("99", "Outras operações"),
    };

    private static readonly (string Codigo, string Descricao)[] Ipi =
    {
        ("00", "Entrada com recuperação de crédito"),
        ("01", "Entrada tributada com alíquota zero"),
        ("02", "Entrada isenta"),
        ("03", "Entrada não tributada"),
        ("04", "Entrada imune"),
        ("05", "Entrada com suspensão"),
        ("49", "Outras entradas"),
        ("50", "Saída tributada"),
        ("51", "Saída tributada com alíquota zero"),
        ("52", "Saída isenta"),
        ("53", "Saída não tributada"),
        ("54", "Saída imune"),
        ("55", "Saída com suspensão"),
        ("99", "Outras saídas"),
    };

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var table in new[] { "csts_icms", "csts_pis", "csts_cofins", "csts_ipi" })
        {
            Exec(connection, transaction, $@"
                CREATE TABLE IF NOT EXISTS {table} (
                    codigo VARCHAR(4) NOT NULL PRIMARY KEY,
                    descricao TEXT NOT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");
        }

        Seed(connection, transaction, "csts_icms", Icms);
        Seed(connection, transaction, "csts_pis", PisCofins);
        Seed(connection, transaction, "csts_cofins", PisCofins);
        Seed(connection, transaction, "csts_ipi", Ipi);
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var table in new[] { "csts_icms", "csts_pis", "csts_cofins", "csts_ipi" })
            Exec(connection, transaction, $"DROP TABLE IF EXISTS {table};");
    }

    private static void Seed(IDbConnection conn, IDbTransaction tx, string table, (string Codigo, string Descricao)[] rows)
    {
        foreach (var (codigo, descricao) in rows)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = $@"INSERT INTO {table} (codigo, descricao) VALUES (@codigo, @descricao)
                                 ON DUPLICATE KEY UPDATE descricao = VALUES(descricao);";
            AddParam(cmd, "@codigo", codigo);
            AddParam(cmd, "@descricao", descricao);
            cmd.ExecuteNonQuery();
        }
    }

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
