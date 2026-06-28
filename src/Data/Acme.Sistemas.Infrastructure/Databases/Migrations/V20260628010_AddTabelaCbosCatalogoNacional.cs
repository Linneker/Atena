using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Cria a tabela de referência <c>cbos</c> (Classificação Brasileira de Ocupações,
/// catálogo nacional, não tenant-scoped). A migration sobe a tabela vazia — popular
/// via endpoint admin <c>POST /api/v1/admin/rh/cbos/seed</c> (opt-in, similar a CFOP).
/// </summary>
public sealed class V20260628010_AddTabelaCbosCatalogoNacional : IMigration
{
    public long Version => 20260628010;
    public string Name => "AddTabelaCbosCatalogoNacional";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS cbos (
                codigo CHAR(6) NOT NULL PRIMARY KEY,
                titulo VARCHAR(255) NOT NULL,
                grande_grupo CHAR(1) NULL,
                familia CHAR(4) NULL,
                ativo TINYINT(1) NOT NULL DEFAULT 1,
                INDEX ix_cbos_familia (familia),
                INDEX ix_cbos_grande_grupo (grande_grupo)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS cbos;");
}
