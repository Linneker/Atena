using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101014_AdicionarFifoEstoque : IMigration
{
    public long Version => 20260101014;
    public string Name => "AdicionarFifoEstoque";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        if (!MigrationHelper.ColumnExists(connection, transaction, "entrada_produto_estoque", "quantidade_restante"))
        {
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE entrada_produto_estoque ADD COLUMN quantidade_restante DECIMAL(15,4) NOT NULL DEFAULT 0;");
            MigrationHelper.Execute(connection, transaction,
                "UPDATE entrada_produto_estoque SET quantidade_restante = quantidade WHERE quantidade_restante = 0;");
            MigrationHelper.Execute(connection, transaction,
                "CREATE INDEX ix_entrada_fifo ON entrada_produto_estoque (tenant_id, estoque_id, produto_id, data_movimento, quantidade_restante);");
        }
        if (!MigrationHelper.ColumnExists(connection, transaction, "saida_produto_estoque", "cmv_unitario"))
        {
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE saida_produto_estoque ADD COLUMN cmv_unitario DECIMAL(15,4) NULL;");
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        if (MigrationHelper.ColumnExists(connection, transaction, "saida_produto_estoque", "cmv_unitario"))
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE saida_produto_estoque DROP COLUMN cmv_unitario;");
        if (MigrationHelper.ColumnExists(connection, transaction, "entrada_produto_estoque", "quantidade_restante"))
        {
            MigrationHelper.Execute(connection, transaction, "DROP INDEX ix_entrada_fifo ON entrada_produto_estoque;");
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE entrada_produto_estoque DROP COLUMN quantidade_restante;");
        }
    }
}
