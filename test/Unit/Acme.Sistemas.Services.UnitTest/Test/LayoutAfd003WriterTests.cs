using System.Text;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Enums.Rh;
using Acme.Sistemas.Services.V1.Rh.Oficial671.Afd;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class LayoutAfd003WriterTests
{
    private static AfdContexto Cenario(int qtdComprovantes = 0)
    {
        var emp = new Empresa
        {
            RazaoSocial = "Acme Brasil LTDA",
            Cnpj = "28088742000130",
            Endereco = new Endereco
            {
                Logradouro = "Rua Acme", Numero = "100",
                Bairro = "Centro", Cidade = "Curitiba", Uf = "PR",
            },
        };
        var cfg = new ConfiguracaoRep { Cno = "12345678901234" };
        var funcs = new List<Funcionario>
        {
            new() { Id = Guid.NewGuid(), NomeCompleto = "Maria Souza",
                Cpf = "12345678900", Pis = "12345678901" },
        };
        var marcacoes = new List<MarcacaoPonto>();
        var comprovantes = new List<ComprovantePonto>();
        for (var i = 0; i < qtdComprovantes; i++)
        {
            var mid = Guid.NewGuid();
            marcacoes.Add(new MarcacaoPonto
            {
                Id = mid, FuncionarioId = funcs[0].Id,
                DataHora = new DateTime(2026, 6, 28, 8, 0, 0, DateTimeKind.Utc).AddMinutes(i * 10),
                Tipo = TipoMarcacao.Entrada, HashIntegridade = $"h{i}",
            });
            comprovantes.Add(new ComprovantePonto
            {
                MarcacaoId = mid, Nsr = i + 1,
                EmpresaId = emp.Id, EmitidoEm = DateTime.UtcNow,
            });
        }
        return new AfdContexto(emp, cfg,
            new DateTime(2026, 6, 1), new DateTime(2026, 6, 30),
            new DateTime(2026, 6, 28, 12, 0, 0, DateTimeKind.Utc),
            marcacoes, comprovantes, funcs);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "LayoutAfd003")]
    [Fact(DisplayName = "Dado contexto sem comprovantes, quando Escrever, então gera cabeçalho + identificador + trailer com totais zero")]
    public void Sem_Comprovantes_GeraEsqueleto()
    {
        var w = new LayoutAfd003Writer();
        var r = w.Escrever(Cenario(0));
        var texto = Encoding.UTF8.GetString(r.Conteudo);
        texto.Should().Contain("28088742000130");           // CNPJ no cabeçalho
        texto.Should().Contain("ATENA-REP-C");              // identificador tipo 2
        texto.Split('\n')[0].Should().StartWith("000000001");// NSR cabeçalho zero-padded
        r.HashSha256Hex.Should().HaveLength(64);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "LayoutAfd003")]
    [Fact(DisplayName = "Dado 3 comprovantes, quando Escrever, então emite 1 empregado e 3 marcações ordenadas por NSR")]
    public void TresComprovantes_EmiteMarcacoesOrdenadas()
    {
        var w = new LayoutAfd003Writer();
        var r = w.Escrever(Cenario(3));
        var linhas = Encoding.UTF8.GetString(r.Conteudo).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        // Esqueleto: cabec + ident + 1 empregado + 3 marcacoes + trailer = 7 linhas
        linhas.Should().HaveCount(7);
        linhas[2][9].Should().Be('5');                      // tipo 5 = empregado
        linhas[3][9].Should().Be('3');                      // tipo 3 = marcação
        linhas[6][9].Should().Be('9');                      // trailer
    }
}
