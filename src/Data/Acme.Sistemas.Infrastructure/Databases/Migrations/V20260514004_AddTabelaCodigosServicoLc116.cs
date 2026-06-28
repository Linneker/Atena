using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Cria a tabela <c>codigos_servico_lc116</c> e semeia um SUBSET CURADO da lista de serviços
/// da Lei Complementar 116/2003 — cobrindo os 40 grupos e os subitens mais usados (TI,
/// engenharia, saúde, administrativo, financeiro). A lista completa (~190 subitens) é um
/// dataset que pode ser drop-in via <c>documentacao/seeds/lc116.json</c> futuramente. Estes
/// códigos também são usados pela change nfse-abrasf-pluggavel.
/// </summary>
public sealed class V20260514004_AddTabelaCodigosServicoLc116 : IMigration
{
    public long Version => 20260514004;
    public string Name => "AddTabelaCodigosServicoLc116";

    private static readonly (string Codigo, string Descricao)[] Codigos =
    {
        ("1.01", "Análise e desenvolvimento de sistemas"),
        ("1.02", "Programação"),
        ("1.03", "Processamento, armazenamento ou hospedagem de dados, textos, imagens e congêneres"),
        ("1.04", "Elaboração de programas de computadores, inclusive de jogos eletrônicos"),
        ("1.05", "Licenciamento ou cessão de direito de uso de programas de computação"),
        ("1.06", "Assessoria e consultoria em informática"),
        ("1.07", "Suporte técnico em informática, inclusive instalação, configuração e manutenção"),
        ("1.08", "Planejamento, confecção, manutenção e atualização de páginas eletrônicas"),
        ("1.09", "Disponibilização de conteúdos de áudio, vídeo, imagem e texto pela internet (streaming)"),
        ("2.01", "Serviços de pesquisas e desenvolvimento de qualquer natureza"),
        ("3.02", "Cessão de direito de uso de marcas e de sinais de propaganda"),
        ("3.03", "Exploração de salões de festas, centro de convenções, escritórios virtuais e congêneres"),
        ("4.01", "Medicina e biomedicina"),
        ("4.03", "Hospitais, clínicas, laboratórios, sanatórios, prontos-socorros e congêneres"),
        ("4.08", "Terapia ocupacional, fisioterapia e fonoaudiologia"),
        ("4.12", "Odontologia"),
        ("4.16", "Psicologia"),
        ("5.01", "Medicina veterinária e zootecnia"),
        ("6.01", "Barbearia, cabeleireiros, manicuros, pedicuros e congêneres"),
        ("6.02", "Esteticistas, tratamento de pele, depilação e congêneres"),
        ("7.01", "Engenharia, agronomia, agrimensura, arquitetura, geologia, urbanismo e congêneres"),
        ("7.02", "Execução de obras de construção civil, hidráulica ou elétrica e semelhantes"),
        ("7.05", "Reparação, conservação e reforma de edifícios, estradas, pontes e congêneres"),
        ("7.10", "Limpeza, manutenção e conservação de imóveis, piscinas, parques, jardins e congêneres"),
        ("7.11", "Decoração e jardinagem, inclusive corte e poda de árvores"),
        ("8.01", "Ensino regular pré-escolar, fundamental, médio e superior"),
        ("8.02", "Instrução, treinamento, orientação pedagógica e educacional, avaliação de conhecimentos"),
        ("9.01", "Hospedagem em hotéis, apart-hotéis, motéis, pensões e congêneres"),
        ("9.02", "Agenciamento, organização e execução de programas de turismo, passeios e excursões"),
        ("10.02", "Agenciamento, corretagem ou intermediação de títulos, valores mobiliários e contratos"),
        ("10.05", "Agenciamento, corretagem ou intermediação de bens móveis ou imóveis"),
        ("10.09", "Representação de qualquer natureza, inclusive comercial"),
        ("11.01", "Guarda e estacionamento de veículos terrestres, aeronaves e embarcações"),
        ("11.02", "Vigilância, segurança ou monitoramento de bens, pessoas e semoventes"),
        ("11.04", "Armazenamento, depósito, carga, descarga, arrumação e guarda de bens"),
        ("12.13", "Produção de eventos, espetáculos, shows, concertos, festivais e congêneres"),
        ("13.03", "Fotografia e cinematografia, inclusive revelação, ampliação, cópia e congêneres"),
        ("13.05", "Composição gráfica, fotocomposição, clicheria, litografia e congêneres"),
        ("14.01", "Lubrificação, limpeza, conserto, restauração e manutenção de máquinas e equipamentos"),
        ("14.02", "Assistência técnica"),
        ("14.06", "Instalação e montagem de aparelhos, máquinas e equipamentos"),
        ("14.13", "Carpintaria e serralheria"),
        ("15.01", "Administração de fundos, consórcio, cartão de crédito ou débito e congêneres"),
        ("15.10", "Serviços relacionados a cobranças, recebimentos ou pagamentos em geral"),
        ("16.01", "Serviços de transporte coletivo municipal de passageiros"),
        ("17.01", "Assessoria ou consultoria de qualquer natureza, não contida em outros itens"),
        ("17.05", "Fornecimento de mão-de-obra, mesmo em caráter temporário"),
        ("17.06", "Propaganda e publicidade, inclusive promoção de vendas e planejamento de campanhas"),
        ("17.14", "Advocacia"),
        ("17.16", "Auditoria"),
        ("17.19", "Contabilidade, inclusive serviços técnicos e auxiliares"),
        ("17.20", "Consultoria e assessoria econômica ou financeira"),
        ("18.01", "Serviços de regulação de sinistros vinculados a contratos de seguros"),
        ("19.01", "Distribuição e venda de bilhetes de loteria, bingos, cartões e congêneres"),
        ("20.01", "Serviços portuários, utilização de porto, atracação, capatazia e congêneres"),
        ("21.01", "Serviços de registros públicos, cartorários e notariais"),
        ("22.01", "Serviços de exploração de rodovia mediante cobrança de pedágio"),
        ("23.01", "Serviços de programação e comunicação visual, desenho industrial e congêneres"),
        ("24.01", "Serviços de chaveiros, confecção de carimbos, placas, sinalização visual e congêneres"),
        ("25.01", "Funerais, inclusive fornecimento de caixão, aluguel de capela e congêneres"),
        ("26.01", "Coleta, remessa ou entrega de correspondências, documentos, bens ou valores; courrier"),
        ("27.01", "Serviços de assistência social"),
        ("28.01", "Serviços de avaliação de bens e serviços de qualquer natureza"),
        ("29.01", "Serviços de biblioteconomia"),
        ("30.01", "Serviços de biologia, biotecnologia e química"),
        ("31.01", "Serviços técnicos em edificações, eletrônica, eletrotécnica, mecânica e telecomunicações"),
        ("32.01", "Serviços de desenhos técnicos"),
        ("33.01", "Serviços de desembaraço aduaneiro, comissários, despachantes e congêneres"),
        ("34.01", "Serviços de investigações particulares, detetives e congêneres"),
        ("35.01", "Serviços de reportagem, assessoria de imprensa, jornalismo e relações públicas"),
        ("36.01", "Serviços de meteorologia"),
        ("37.01", "Serviços de artistas, atletas, modelos e manequins"),
        ("38.01", "Serviços de museologia"),
        ("39.01", "Serviços de ourivesaria e lapidação"),
        ("40.01", "Obras de arte sob encomenda"),
    };

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS codigos_servico_lc116 (
                codigo VARCHAR(10) NOT NULL PRIMARY KEY,
                descricao TEXT NOT NULL
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");

        foreach (var (codigo, descricao) in Codigos)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"INSERT INTO codigos_servico_lc116 (codigo, descricao) VALUES (@codigo, @descricao)
                                ON DUPLICATE KEY UPDATE descricao = VALUES(descricao);";
            AddParam(cmd, "@codigo", codigo);
            AddParam(cmd, "@descricao", descricao);
            cmd.ExecuteNonQuery();
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => Exec(connection, transaction, "DROP TABLE IF EXISTS codigos_servico_lc116;");

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
