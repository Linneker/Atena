using Acme.Sistemas.Domain.Interfaces.Rh;
using Acme.Sistemas.Services.V1.Rh.Oficial671.Servicos;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class GeradorComprovantePontoTextoTests
{
    [Trait("Solucao", "Services")]
    [Trait("Acao", "GeradorComprovantePontoTexto")]
    [Fact(DisplayName = "Dado dados completos, quando Gerar, então retorna linha pipe-separated conforme anexo II")]
    public void Gerar_LayoutPipe()
    {
        var g = new GeradorComprovantePontoTexto();
        var s = g.Gerar(new DadosComprovante671(
            Nsr: 42,
            TipoRegistro: "Entrada",
            CpfEmpregado: "123.456.789-00",
            PisEmpregado: "123.45678.90-1",
            DataHora: new DateTime(2026, 6, 28, 9, 30, 15, DateTimeKind.Utc),
            NomeEmpregado: "MARIA DA SILVA SOUZA",
            CnpjEmpregador: "28.088.742/0001-30",
            HashEncadeadoMarcacao: "deadbeef"));

        var partes = s.Split('|');
        partes[0].Should().Be("000000042");
        partes[1].Should().Be("Entrada");
        partes[2].Should().Be("12345678900");
        partes[3].Should().Be("12345678901");
        partes[4].Should().Be("20260628");
        partes[5].Should().Be("093015");
        partes[6].Should().Be("MARIA DA SILVA SOUZA");
        partes[7].Should().Be("28088742000130");
        partes[8].Should().Be("deadbeef");
    }
}
