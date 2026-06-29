using System.Text.Json;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;
using Acme.Sistemas.Services.V1.Rh.Oficial671.Aej;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class GeradorAejV1Tests
{
    [Trait("Solucao", "Services")]
    [Trait("Acao", "GeradorAejV1")]
    [Fact(DisplayName = "Dado contexto vazio, quando Gerar, então retorna JSON com seções obrigatórias")]
    public void Gerar_TodasSecoesPresentes()
    {
        var g = new GeradorAejV1();
        var ctx = new AejContexto(
            new Empresa { RazaoSocial = "Acme", Cnpj = "28088742000130" },
            new ConfiguracaoRep(),
            new DateTime(2026, 6, 1), new DateTime(2026, 6, 30),
            new DateTime(2026, 6, 28, 12, 0, 0, DateTimeKind.Utc),
            new List<MarcacaoPonto>(),
            new List<ComprovantePonto>(),
            new List<Funcionario>());
        var bytes = g.Gerar(ctx);
        using var doc = JsonDocument.Parse(bytes);
        var root = doc.RootElement;
        root.GetProperty("cabecalho").GetProperty("empregadorCnpj").GetString().Should().Be("28088742000130");
        root.GetProperty("jornadas").GetArrayLength().Should().Be(0);
        root.GetProperty("bancosHoras").GetArrayLength().Should().Be(0);
        root.GetProperty("marcacoes").GetArrayLength().Should().Be(0);
        root.GetProperty("ajustes").GetArrayLength().Should().Be(0);
        root.GetProperty("espelhos").GetArrayLength().Should().Be(0);
    }
}
