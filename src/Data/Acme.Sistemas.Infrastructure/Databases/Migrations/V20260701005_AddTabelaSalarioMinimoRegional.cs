using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Salário-mínimo regional por UF — opt-in via upload admin (RS, PR, SC, SP têm pisos regionais).
/// Tabela vazia no seed; tenant que precisa carregar pelos endpoints do W5 Fase 3.
/// </summary>
public sealed class V20260701005_AddTabelaSalarioMinimoRegional : IMigration
{
    public long Version => 20260701005;
    public string Name => "AddTabelaSalarioMinimoRegional";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS salario_minimo_regional (
                id CHAR(36) NOT NULL PRIMARY KEY,
                uf CHAR(2) NOT NULL,
                competencia_inicio CHAR(7) NOT NULL,
                competencia_fim CHAR(7) NULL,
                faixa_descricao VARCHAR(200) NOT NULL,
                valor DECIMAL(10,2) NOT NULL,
                seed_origem VARCHAR(20) NOT NULL DEFAULT 'upload-admin',
                importado_em DATETIME(6) NOT NULL,
                importado_por CHAR(36) NULL,
                INDEX ix_sm_reg_uf_vigencia (uf, competencia_inicio, competencia_fim)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS salario_minimo_regional;");
}
