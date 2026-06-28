using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Adiciona origem_despesa_id em despesas e origem_receita_id em receitas para
/// rastrear instâncias geradas pelo RecorrenciaFinanceiraWorker. Permite navegar
/// das entries mensais até o template (DespesaFixa=true).
/// </summary>
public sealed class V20260513001_AdicionarOrigemRecorrenciaFinanceiro : IMigration
{
    public long Version => 20260513001;
    public string Name => "AdicionarOrigemRecorrenciaFinanceiro";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        Exec(connection, transaction, @"
            ALTER TABLE despesas
                ADD COLUMN origem_despesa_id CHAR(36) NULL AFTER fornecedor_id,
                ADD INDEX idx_despesas_origem (tenant_id, origem_despesa_id);
        ");

        Exec(connection, transaction, @"
            ALTER TABLE receitas
                ADD COLUMN origem_receita_id CHAR(36) NULL AFTER origem_venda_id,
                ADD INDEX idx_receitas_origem (tenant_id, origem_receita_id);
        ");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        Exec(connection, transaction, "ALTER TABLE despesas DROP INDEX idx_despesas_origem, DROP COLUMN origem_despesa_id;");
        Exec(connection, transaction, "ALTER TABLE receitas DROP INDEX idx_receitas_origem, DROP COLUMN origem_receita_id;");
    }

    private static void Exec(IDbConnection conn, IDbTransaction tx, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}
