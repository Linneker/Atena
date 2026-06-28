using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Cria <c>Usuario</c> com Status=Inativo (0) e vincula a cada <c>Funcionario</c> ativo
/// que ainda não tem <c>usuario_id</c>. O e-mail gerado é único e contém o id do
/// funcionário (<c>func-{id}@auto.local</c>) para que o admin do tenant identifique e
/// edite depois. O <c>password_hash</c> é uma string sentinela que jamais valida em login.
/// </summary>
public sealed class V20260628013_CriarUsuariosDesativadosParaFuncionariosLegados : IMigration
{
    public long Version => 20260628013;
    public string Name => "CriarUsuariosDesativadosParaFuncionariosLegados";

    private const string SentinelaHash = "!INATIVO-AUTO-CRIADO-MIGRATION-V20260628013!";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        // 1) Cria usuários inativos (status=0) para cada funcionário ativo sem usuario_id.
        MigrationHelper.Execute(connection, transaction, $@"
            INSERT INTO usuarios (id, tenant_id, nome_completo, email, password_hash, status, failed_login_attempts, created_at)
            SELECT
                UUID(),
                f.tenant_id,
                f.nome_completo,
                CONCAT('func-', REPLACE(f.id, '-', ''), '@auto.local'),
                '{SentinelaHash}',
                0,
                0,
                UTC_TIMESTAMP()
            FROM funcionarios f
            WHERE f.usuario_id IS NULL
              AND f.status = 1
              AND f.deleted_at IS NULL;");

        // 2) Vincula cada funcionário ao usuário recém-criado pela convenção de e-mail.
        MigrationHelper.Execute(connection, transaction, @"
            UPDATE funcionarios f
            JOIN usuarios u
              ON u.tenant_id = f.tenant_id
             AND u.email = CONCAT('func-', REPLACE(f.id, '-', ''), '@auto.local')
            SET f.usuario_id = u.id
            WHERE f.usuario_id IS NULL
              AND f.status = 1
              AND f.deleted_at IS NULL;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        // Remove apenas os usuários auto-criados (identificáveis pelo hash sentinela).
        MigrationHelper.Execute(connection, transaction, $@"
            UPDATE funcionarios f
            JOIN usuarios u ON u.id = f.usuario_id
            SET f.usuario_id = NULL
            WHERE u.password_hash = '{SentinelaHash}';");

        MigrationHelper.Execute(connection, transaction, $@"
            DELETE FROM usuarios WHERE password_hash = '{SentinelaHash}';");
    }
}
