using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Naturezas de rubrica do eSocial (evento S-1010 — Tabela de Rubricas).
/// Catálogo oficial: códigos 1xxx = proventos (vencimentos); 9xxx = descontos; 5xxx = informativas.
/// Seed traz subset (~70 códigos mais usados) — catálogo completo (~700) via upload admin.
/// </summary>
public sealed class V20260701008_AddTabelaNaturezasRubricaEsocial : IMigration
{
    public long Version => 20260701008;
    public string Name => "AddTabelaNaturezasRubricaEsocial";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS naturezas_rubrica_esocial (
                codigo VARCHAR(10) NOT NULL PRIMARY KEY,
                descricao VARCHAR(500) NOT NULL,
                tipo_grupo VARCHAR(20) NOT NULL,
                ativa TINYINT(1) NOT NULL DEFAULT 1,
                seed_origem VARCHAR(20) NOT NULL DEFAULT 'migration',
                importado_em DATETIME(6) NOT NULL,
                INDEX ix_nat_esocial_grupo (tipo_grupo)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        var naturezas = new (string Codigo, string Descricao, string Grupo)[]
        {
            ("1000", "Salário/vencimento/subsídio/soldo, inclusive 13º", "Provento"),
            ("1010", "Vantagens e adicionais habituais", "Provento"),
            ("1020", "Horas extras", "Provento"),
            ("1030", "Adicional noturno", "Provento"),
            ("1040", "Adicional de insalubridade", "Provento"),
            ("1050", "Adicional de periculosidade", "Provento"),
            ("1060", "Adicional de transferência", "Provento"),
            ("1070", "Adicional por tempo de serviço/anuênio/quinquênio", "Provento"),
            ("1080", "Demais adicionais habituais", "Provento"),
            ("1090", "Quebra de caixa", "Provento"),
            ("1110", "Prêmios habituais", "Provento"),
            ("1120", "Gratificações habituais", "Provento"),
            ("1130", "Comissões e percentagens habituais", "Provento"),
            ("1199", "Outras verbas de natureza salarial", "Provento"),
            ("1200", "Diárias de viagem (excedente a 50% da remuneração)", "Provento"),
            ("1601", "13º salário (proporcional ou integral)", "Provento"),
            ("1620", "Férias gozadas (remuneração)", "Provento"),
            ("1622", "Adicional de 1/3 sobre férias gozadas", "Provento"),
            ("1623", "Abono pecuniário de férias (1/3 vendido)", "Provento"),
            ("1624", "Adicional de 1/3 sobre abono pecuniário", "Provento"),
            ("1801", "DSR sobre verbas salariais variáveis", "Provento"),
            ("1802", "DSR sobre horas extras", "Provento"),
            ("2501", "Aviso prévio indenizado", "Provento"),
            ("2502", "13º proporcional no aviso prévio indenizado", "Provento"),
            ("2503", "Indenização do art. 479 da CLT (rescisão antecipada de contrato a prazo)", "Provento"),
            ("2599", "Outras indenizações decorrentes da rescisão", "Provento"),
            ("3501", "Salário-família", "Provento"),
            ("3502", "Salário-família a maior", "Provento"),
            ("3503", "Auxílio-doença previdenciário (15 primeiros dias)", "Provento"),
            ("3504", "Auxílio-acidente", "Provento"),
            ("3505", "Auxílio-creche", "Provento"),
            ("3506", "Auxílio-babá", "Provento"),
            ("5001", "Vale-transporte (parcela paga pelo empregador)", "Informativa"),
            ("5002", "Vale-refeição/alimentação fornecido nos termos do PAT", "Informativa"),
            ("5003", "Plano de saúde (parcela patronal)", "Informativa"),
            ("5004", "Plano odontológico (parcela patronal)", "Informativa"),
            ("5005", "Seguro de vida em grupo", "Informativa"),
            ("5006", "Previdência privada complementar (parcela patronal)", "Informativa"),
            ("5007", "Educação (custeio de cursos/livros para empregado)", "Informativa"),
            ("5008", "Vestuário/uniforme/EPI", "Informativa"),
            ("5009", "PLR — Participação nos Lucros e Resultados", "Informativa"),
            ("9201", "Contribuição previdenciária (INSS) — segurado", "Desconto"),
            ("9202", "Imposto de renda retido na fonte (IRRF)", "Desconto"),
            ("9203", "Contribuição sindical", "Desconto"),
            ("9204", "Contribuição assistencial/confederativa", "Desconto"),
            ("9205", "Contribuição associativa", "Desconto"),
            ("9206", "Pensão alimentícia judicial", "Desconto"),
            ("9207", "Empréstimo consignado em folha", "Desconto"),
            ("9208", "Mensalidade sindical", "Desconto"),
            ("9210", "Vale-transporte (parcela descontada do empregado, máx 6%)", "Desconto"),
            ("9211", "Vale-refeição (parcela descontada)", "Desconto"),
            ("9212", "Plano de saúde (parcela do empregado)", "Desconto"),
            ("9213", "Plano odontológico (parcela do empregado)", "Desconto"),
            ("9214", "Convênio farmácia/ótica/supermercado", "Desconto"),
            ("9215", "Faltas injustificadas (desconto)", "Desconto"),
            ("9216", "Atrasos e saídas antecipadas", "Desconto"),
            ("9217", "DSR perdido por falta/atraso injustificado", "Desconto"),
            ("9218", "Adiantamento salarial (vale)", "Desconto"),
            ("9219", "Adiantamento de 13º salário", "Desconto"),
            ("9220", "Adiantamento de férias", "Desconto"),
            ("9221", "Cesta básica (parcela descontada)", "Desconto"),
            ("9222", "Seguro de vida (parcela do empregado)", "Desconto"),
            ("9223", "Previdência privada (parcela do empregado)", "Desconto"),
            ("9230", "Aviso prévio (descontado por pedido de demissão sem aviso)", "Desconto"),
            ("9231", "Indenização de devolução de equipamentos não devolvidos", "Desconto"),
            ("9298", "Diversos (desconto autorizado em ACT/CCT/contrato)", "Desconto"),
            ("9299", "Outras verbas descontadas — não especificadas", "Desconto"),
            ("9901", "FGTS depositado pelo empregador (informativo)", "Informativa"),
            ("9902", "Multa rescisória FGTS 40% (informativo)", "Informativa"),
            ("9903", "Banco de horas — saldo (informativo)", "Informativa"),
            ("9904", "Banco de horas — compensação (informativo)", "Informativa"),
            ("9999", "Outras verbas — natureza a definir", "Informativa"),
        };

        foreach (var n in naturezas)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"
                INSERT INTO naturezas_rubrica_esocial (codigo, descricao, tipo_grupo, ativa, seed_origem, importado_em)
                VALUES (@cod, @desc, @grp, 1, 'migration', UTC_TIMESTAMP(6))
                ON DUPLICATE KEY UPDATE descricao = VALUES(descricao), tipo_grupo = VALUES(tipo_grupo);";
            AddParam(cmd, "@cod", n.Codigo);
            AddParam(cmd, "@desc", n.Descricao);
            AddParam(cmd, "@grp", n.Grupo);
            cmd.ExecuteNonQuery();
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS naturezas_rubrica_esocial;");

    private static void AddParam(IDbCommand cmd, string name, object value)
    {
        var p = cmd.CreateParameter();
        p.ParameterName = name;
        p.Value = value;
        cmd.Parameters.Add(p);
    }
}
