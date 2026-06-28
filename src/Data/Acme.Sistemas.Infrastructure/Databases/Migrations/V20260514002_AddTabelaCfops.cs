using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Cria a tabela de referência <c>cfops</c> e semeia um SUBSET CURADO dos CFOPs mais usados
/// (venda/compra/devolução/remessa, dentro e fora do estado, exportação). A lista oficial
/// completa da Receita Federal (~700 CFOPs) é um dataset externo: deve ser carregada via
/// <c>documentacao/seeds/cfops.json</c> (ver README na pasta de seeds) — task 1.2.2/1.2.3
/// marcada como BLOQUEADO até o JSON oficial ser fornecido. O <c>seed_version</c> permite
/// substituir o subset pelo dataset completo sem conflito.
/// </summary>
public sealed class V20260514002_AddTabelaCfops : IMigration
{
    public long Version => 20260514002;
    public string Name => "AddTabelaCfops";

    private const int SeedVersion = 1;

    private static readonly (string Codigo, string Categoria, string Descricao)[] Cfops =
    {
        // Entradas — internas (1xxx)
        ("1101", "Entrada", "Compra para industrialização ou produção rural"),
        ("1102", "Entrada", "Compra para comercialização"),
        ("1124", "Entrada", "Industrialização efetuada por outra empresa"),
        ("1201", "Entrada", "Devolução de venda de produção do estabelecimento"),
        ("1202", "Entrada", "Devolução de venda de mercadoria adquirida ou recebida de terceiros"),
        ("1252", "Entrada", "Compra de energia elétrica para estabelecimento comercial"),
        ("1403", "Entrada", "Compra para comercialização em operação com mercadoria sujeita a ST"),
        ("1556", "Entrada", "Compra de material para uso ou consumo"),
        ("1551", "Entrada", "Compra de bem para o ativo imobilizado"),
        ("1933", "Entrada", "Aquisição de serviço tributado pelo ISSQN"),
        // Entradas — interestaduais (2xxx)
        ("2101", "Entrada", "Compra para industrialização ou produção rural (interestadual)"),
        ("2102", "Entrada", "Compra para comercialização (interestadual)"),
        ("2202", "Entrada", "Devolução de venda de mercadoria adquirida de terceiros (interestadual)"),
        ("2551", "Entrada", "Compra de bem para o ativo imobilizado (interestadual)"),
        // Entradas — exterior (3xxx)
        ("3101", "Entrada", "Compra para industrialização ou produção rural (importação)"),
        ("3102", "Entrada", "Compra para comercialização (importação)"),
        // Saídas — internas (5xxx)
        ("5101", "Saida", "Venda de produção do estabelecimento"),
        ("5102", "Saida", "Venda de mercadoria adquirida ou recebida de terceiros"),
        ("5103", "Saida", "Venda de produção do estabelecimento efetuada fora do estabelecimento"),
        ("5201", "Saida", "Devolução de compra para industrialização ou produção rural"),
        ("5202", "Saida", "Devolução de compra para comercialização"),
        ("5405", "Saida", "Venda de mercadoria adquirida de terceiros sujeita a ST (substituto)"),
        ("5551", "Saida", "Venda de bem do ativo imobilizado"),
        ("5910", "Saida", "Remessa em bonificação, doação ou brinde"),
        ("5915", "Saida", "Remessa de mercadoria para conserto ou reparo"),
        ("5933", "Saida", "Prestação de serviço tributado pelo ISSQN"),
        // Saídas — interestaduais (6xxx)
        ("6101", "Saida", "Venda de produção do estabelecimento (interestadual)"),
        ("6102", "Saida", "Venda de mercadoria adquirida ou recebida de terceiros (interestadual)"),
        ("6108", "Saida", "Venda de mercadoria a não contribuinte (interestadual)"),
        ("6202", "Saida", "Devolução de compra para comercialização (interestadual)"),
        ("6404", "Saida", "Venda de mercadoria sujeita a ST (interestadual)"),
        // Saídas — exterior (7xxx)
        ("7101", "Saida", "Venda de produção do estabelecimento (exportação)"),
        ("7102", "Saida", "Venda de mercadoria adquirida ou recebida de terceiros (exportação)"),
    };

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS cfops (
                codigo CHAR(4) NOT NULL PRIMARY KEY,
                descricao TEXT NOT NULL,
                categoria VARCHAR(20) NOT NULL,
                seed_version INT NOT NULL DEFAULT 1,
                INDEX idx_cfops_categoria (categoria)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");

        foreach (var (codigo, categoria, descricao) in Cfops)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"INSERT INTO cfops (codigo, descricao, categoria, seed_version)
                                VALUES (@codigo, @descricao, @categoria, @sv)
                                ON DUPLICATE KEY UPDATE descricao = VALUES(descricao),
                                    categoria = VALUES(categoria), seed_version = VALUES(seed_version);";
            AddParam(cmd, "@codigo", codigo);
            AddParam(cmd, "@descricao", descricao);
            AddParam(cmd, "@categoria", categoria);
            AddParam(cmd, "@sv", SeedVersion);
            cmd.ExecuteNonQuery();
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => Exec(connection, transaction, "DROP TABLE IF EXISTS cfops;");

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
