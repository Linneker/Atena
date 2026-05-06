using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101017_CriarTabelasFiscalNFe : IMigration
{
    public long Version => 20260101017;
    public string Name => "CriarTabelasFiscalNFe";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS configuracao_fiscal (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                ambiente TINYINT NOT NULL DEFAULT 2,
                modo TINYINT NOT NULL DEFAULT 1,
                uf VARCHAR(2) NOT NULL DEFAULT 'SP',
                cnpj_emitente VARCHAR(20) NOT NULL,
                razao_social_emitente VARCHAR(255) NULL,
                inscricao_estadual VARCHAR(50) NULL,
                serie_nfe INT NOT NULL DEFAULT 1,
                proximo_numero INT NOT NULL DEFAULT 1,
                certificado_pfx_criptografado MEDIUMBLOB NULL,
                certificado_nonce_base64 VARCHAR(64) NULL,
                certificado_subject VARCHAR(500) NULL,
                certificado_valido_ate DATETIME NULL,
                certificado_senha_criptografada VARCHAR(512) NULL,
                certificado_senha_nonce_base64 VARCHAR(64) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_cf_tenant (tenant_id),
                CONSTRAINT fk_cf_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS nfes (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                numero INT NOT NULL,
                serie INT NOT NULL,
                chave_acesso VARCHAR(50) NULL,
                faturamento_id CHAR(36) NULL,
                cliente_id CHAR(36) NOT NULL,
                ambiente TINYINT NOT NULL,
                modo TINYINT NOT NULL DEFAULT 1,
                data_emissao DATETIME NOT NULL,
                data_autorizacao DATETIME NULL,
                status TINYINT NOT NULL DEFAULT 0,
                protocolo_autorizacao VARCHAR(50) NULL,
                codigo_status_sefaz VARCHAR(10) NULL,
                motivo_sefaz VARCHAR(500) NULL,
                valor_total DECIMAL(15,2) NOT NULL DEFAULT 0,
                xml_autorizado_url VARCHAR(500) NULL,
                xml_enviado_hash VARCHAR(128) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_nfe_tenant_serie_numero (tenant_id, serie, numero),
                INDEX ix_nfe_tenant (tenant_id),
                INDEX ix_nfe_status (tenant_id, status),
                INDEX ix_nfe_chave (chave_acesso),
                INDEX ix_nfe_fat (tenant_id, faturamento_id),
                CONSTRAINT fk_nfe_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS nfe_itens (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                nfe_id CHAR(36) NOT NULL,
                numero_item INT NOT NULL,
                produto_id CHAR(36) NOT NULL,
                descricao VARCHAR(500) NOT NULL,
                quantidade DECIMAL(15,4) NOT NULL,
                preco_unitario DECIMAL(15,4) NOT NULL,
                cfop VARCHAR(10) NULL,
                ncm VARCHAR(10) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_nfei_tenant (tenant_id),
                INDEX ix_nfei_nfe (tenant_id, nfe_id),
                CONSTRAINT fk_nfei_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_nfei_nfe FOREIGN KEY (nfe_id) REFERENCES nfes(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS nfe_eventos (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                nfe_id CHAR(36) NOT NULL,
                tipo INT NOT NULL,
                sequencia INT NOT NULL DEFAULT 1,
                data_evento DATETIME NOT NULL,
                descricao VARCHAR(2000) NULL,
                protocolo_autorizacao VARCHAR(50) NULL,
                codigo_status_sefaz VARCHAR(10) NULL,
                motivo_sefaz VARCHAR(500) NULL,
                xml_evento_url VARCHAR(500) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_nfe_evt_tenant (tenant_id),
                INDEX ix_nfe_evt_nfe (tenant_id, nfe_id, tipo),
                CONSTRAINT fk_nfe_evt_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_nfe_evt_nfe FOREIGN KEY (nfe_id) REFERENCES nfes(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var t in new[] { "nfe_eventos", "nfe_itens", "nfes", "configuracao_fiscal" })
            Exec(connection, transaction, $"DROP TABLE IF EXISTS {t};");
    }

    private static void Exec(IDbConnection c, IDbTransaction t, string sql) =>
        MigrationHelper.Execute(c, t, sql);
}
