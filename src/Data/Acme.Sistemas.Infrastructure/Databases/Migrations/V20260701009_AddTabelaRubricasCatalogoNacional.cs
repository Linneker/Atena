using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Catálogo nacional de rubricas modelo. Read-only. Tenant pode clonar para sua tabela
/// <c>rubricas_tenant</c> e personalizar fórmula/incidências.
/// Seed traz ~30 rubricas modelo: salário-base, HE 50%/100%, adicionais, INSS/IRRF/FGTS, VT/VR,
/// férias, 13º, DSR, salário-família, descontos típicos.
/// </summary>
public sealed class V20260701009_AddTabelaRubricasCatalogoNacional : IMigration
{
    public long Version => 20260701009;
    public string Name => "AddTabelaRubricasCatalogoNacional";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS rubricas_catalogo_nacional (
                codigo VARCHAR(30) NOT NULL PRIMARY KEY,
                descricao VARCHAR(200) NOT NULL,
                tipo VARCHAR(20) NOT NULL,
                natureza_esocial_codigo VARCHAR(10) NULL,
                formula_dsl TEXT NOT NULL,
                incide_inss TINYINT(1) NOT NULL DEFAULT 0,
                incide_irrf TINYINT(1) NOT NULL DEFAULT 0,
                incide_fgts TINYINT(1) NOT NULL DEFAULT 0,
                incide_ferias TINYINT(1) NOT NULL DEFAULT 0,
                incide_13o TINYINT(1) NOT NULL DEFAULT 0,
                incide_dsr TINYINT(1) NOT NULL DEFAULT 0,
                dependencias_json JSON NULL,
                seed_origem VARCHAR(20) NOT NULL DEFAULT 'migration',
                importado_em DATETIME(6) NOT NULL,
                CONSTRAINT fk_rcn_natureza FOREIGN KEY (natureza_esocial_codigo)
                    REFERENCES naturezas_rubrica_esocial(codigo) ON DELETE SET NULL
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        // (codigo, descricao, tipo, natureza, formulaDsl, INSS,IRRF,FGTS,Fer,13o,DSR, dependencias)
        var rubricas = new (string Cod, string Desc, string Tipo, string? Nat, string Dsl,
                            int InINSS, int InIRRF, int InFGTS, int InFer, int In13, int InDSR,
                            string? DepJson)[]
        {
            ("SAL-BASE",   "Salário-base",                                  "Provento",   "1000", "salarioBase",                                                       1,1,1,1,1,1, null),
            ("HE-50",      "Hora extra 50%",                                "Provento",   "1020", "(salarioBase / jornadaHorasMensais) * horasExtras50 * 1.5",         1,1,1,1,1,1, null),
            ("HE-100",     "Hora extra 100%",                               "Provento",   "1020", "(salarioBase / jornadaHorasMensais) * horasExtras100 * 2.0",        1,1,1,1,1,1, null),
            ("HE-70",      "Hora extra 70%",                                "Provento",   "1020", "(salarioBase / jornadaHorasMensais) * horasExtras70 * 1.7",         1,1,1,1,1,1, null),
            ("ADC-NOT",    "Adicional noturno (20%)",                       "Provento",   "1030", "(salarioBase / jornadaHorasMensais) * horasNoturnas * 0.2",         1,1,1,1,1,1, null),
            ("ADC-PER",    "Adicional periculosidade (30%)",                "Provento",   "1050", "salarioBase * 0.3",                                                 1,1,1,1,1,1, null),
            ("ADC-INS",    "Adicional insalubridade (sobre SM)",            "Provento",   "1040", "salarioMinimoVigente * percentualInsalubridade",                    1,1,1,1,1,1, null),
            ("ADC-TS",     "Adicional tempo de serviço",                    "Provento",   "1070", "salarioBase * (anosServico * 0.01)",                                1,1,1,1,1,1, null),
            ("DSR-HE",     "DSR sobre horas extras",                        "Provento",   "1802", "(vlr['HE-50'] + vlr['HE-100']) * (diasDsr / diasUteisMes)",         1,1,1,1,1,0, "[\"HE-50\",\"HE-100\"]"),
            ("QBR-CX",     "Quebra de caixa",                               "Provento",   "1090", "salarioMinimoVigente * 0.1",                                        1,1,1,1,1,1, null),
            ("COM",        "Comissão",                                      "Provento",   "1130", "valorComissaoApurada",                                              1,1,1,1,1,1, null),
            ("PREMIO",     "Prêmio meta",                                   "Provento",   "1110", "if(metaAtingida, salarioBase * 0.1, 0)",                            1,1,1,1,1,1, null),
            ("FERIAS",     "Férias gozadas",                                "Provento",   "1620", "salarioBase",                                                       1,1,1,0,0,0, null),
            ("FERIAS-13",  "1/3 sobre férias",                              "Provento",   "1622", "vlr['FERIAS'] / 3",                                                 1,1,1,0,0,0, "[\"FERIAS\"]"),
            ("DECIMO-3",   "13º salário (proporcional)",                    "Provento",   "1601", "salarioBase * (mesesTrabalhadosAno / 12)",                          1,1,1,0,0,0, null),
            ("SAL-FAM",    "Salário-família",                               "Provento",   "3501", "if(salarioBase <= limiteSalarioFamilia, valorCotaSf * qtdDependentesSf, 0)", 0,0,0,0,0,0, null),
            ("AVISO-IND",  "Aviso prévio indenizado",                       "Provento",   "2501", "salarioBase",                                                       0,1,1,0,0,0, null),
            ("INSS",       "INSS (segurado)",                               "Desconto",   "9201", "aplicaTabelaInss(remuneracaoBruta, competencia)",                   0,0,0,0,0,0, null),
            ("IRRF",       "Imposto de renda retido na fonte",              "Desconto",   "9202", "aplicaTabelaIrrf(remuneracaoBruta - vlr['INSS'], qtdDependentesIrrf, competencia)", 0,0,0,0,0,0, "[\"INSS\"]"),
            ("VT-DESC",    "Vale-transporte (desconto)",                    "Desconto",   "9210", "min(salarioBase * 0.06, valorVtPagoEmpregado)",                     0,0,0,0,0,0, null),
            ("VR-DESC",    "Vale-refeição (desconto)",                      "Desconto",   "9211", "valorVrDesconto",                                                   0,0,0,0,0,0, null),
            ("PS-DESC",    "Plano de saúde (desconto)",                     "Desconto",   "9212", "valorPlanoSaudeDesconto",                                           0,0,0,0,0,0, null),
            ("FALTAS",     "Faltas (desconto)",                             "Desconto",   "9215", "(salarioBase / 30) * diasFaltas",                                   0,0,0,0,0,0, null),
            ("ATRASOS",    "Atrasos (desconto)",                            "Desconto",   "9216", "(salarioBase / jornadaHorasMensais) * (horasAtraso + horasFalta)",  0,0,0,0,0,0, null),
            ("DSR-PERD",   "DSR perdido por falta",                         "Desconto",   "9217", "if(diasFaltas > 0, (salarioBase / 30) * diasDsrPerdido, 0)",        0,0,0,0,0,0, null),
            ("EMP-CONS",   "Empréstimo consignado",                         "Desconto",   "9207", "valorParcelaConsignado",                                            0,0,0,0,0,0, null),
            ("PEN-ALI",    "Pensão alimentícia",                            "Desconto",   "9206", "valorPensaoAlimenticia",                                            0,0,0,0,0,0, null),
            ("CONT-SIND",  "Contribuição sindical",                         "Desconto",   "9204", "valorContribuicaoSindical",                                         0,0,0,0,0,0, null),
            ("FGTS-INFO",  "FGTS depositado (informativo)",                 "Informativa","9901", "remuneracaoBruta * 0.08",                                           0,0,0,0,0,0, null),
            ("BH-SALDO",   "Banco de horas — saldo (informativo)",          "Informativa","9903", "saldoBancoHorasMinutos",                                            0,0,0,0,0,0, null),
        };

        foreach (var r in rubricas)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"
                INSERT INTO rubricas_catalogo_nacional
                    (codigo, descricao, tipo, natureza_esocial_codigo, formula_dsl,
                     incide_inss, incide_irrf, incide_fgts, incide_ferias, incide_13o, incide_dsr,
                     dependencias_json, seed_origem, importado_em)
                VALUES
                    (@cod, @desc, @tipo, @nat, @dsl, @i_inss, @i_irrf, @i_fgts, @i_fer, @i_13, @i_dsr, @dep, 'migration', UTC_TIMESTAMP(6))
                ON DUPLICATE KEY UPDATE
                    descricao = VALUES(descricao),
                    formula_dsl = VALUES(formula_dsl);";
            AddParam(cmd, "@cod", r.Cod);
            AddParam(cmd, "@desc", r.Desc);
            AddParam(cmd, "@tipo", r.Tipo);
            AddParam(cmd, "@nat", (object?)r.Nat ?? DBNull.Value);
            AddParam(cmd, "@dsl", r.Dsl);
            AddParam(cmd, "@i_inss", r.InINSS);
            AddParam(cmd, "@i_irrf", r.InIRRF);
            AddParam(cmd, "@i_fgts", r.InFGTS);
            AddParam(cmd, "@i_fer", r.InFer);
            AddParam(cmd, "@i_13", r.In13);
            AddParam(cmd, "@i_dsr", r.InDSR);
            AddParam(cmd, "@dep", (object?)r.DepJson ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS rubricas_catalogo_nacional;");

    private static void AddParam(IDbCommand cmd, string name, object value)
    {
        var p = cmd.CreateParameter();
        p.ParameterName = name;
        p.Value = value;
        cmd.Parameters.Add(p);
    }
}
