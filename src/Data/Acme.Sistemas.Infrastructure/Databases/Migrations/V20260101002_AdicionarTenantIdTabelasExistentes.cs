using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101002_AdicionarTenantIdTabelasExistentes : IMigration
{
    public long Version => 20260101002;
    public string Name => "AdicionarTenantIdTabelasExistentes";

    private static readonly string[] LegacyTables =
    {
        "Despesa", "Receita", "Divida", "Pagamento", "PagamentoFormaPagamento",
        "PagamentoVenda", "PagamentoVendaFormaPagamento", "FormaPagamento",
        "FluxoDeCaixa", "FluxoDeCaixaDespesa", "FluxoDeCaixaReceita",
        "Empresa", "Fornecedor", "Cliente",
        "Produto", "TipoProduto", "TipoValorProduto", "ValorProduto", "Sequence_Codigo_Produto",
        "Compra", "CompraProduto", "DevolucaoCompra", "DevolucaoVenda",
        "Estoque", "EstoqueProduto", "EntradaProdutoEstoque", "EntradaProdutoEstoqueFornecedor",
        "EntradaProdutoEstoqueDocumento", "SaidaProdutoEstoque", "FluxoCaixaEstoque",
        "Documento", "TipoDocumento", "Competencia", "AutorizacaoApi"
    };

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var table in LegacyTables)
        {
            if (!MigrationHelper.TableExists(connection, transaction, table)) continue;
            if (MigrationHelper.ColumnExists(connection, transaction, table, "tenant_id")) continue;

            MigrationHelper.Execute(connection, transaction,
                $"ALTER TABLE `{table}` ADD COLUMN tenant_id CHAR(36) NULL;");

            MigrationHelper.Execute(connection, transaction,
                $"CREATE INDEX ix_{table.ToLower()}_tenant_id ON `{table}` (tenant_id);");
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var table in LegacyTables)
        {
            if (!MigrationHelper.TableExists(connection, transaction, table)) continue;
            if (!MigrationHelper.ColumnExists(connection, transaction, table, "tenant_id")) continue;
            MigrationHelper.Execute(connection, transaction,
                $"DROP INDEX ix_{table.ToLower()}_tenant_id ON `{table}`;");
            MigrationHelper.Execute(connection, transaction,
                $"ALTER TABLE `{table}` DROP COLUMN tenant_id;");
        }
    }
}
