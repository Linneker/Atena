using Acme.Sistemas.Atena.Mobile.Shared.Helpers;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Atena.Mobile.Tests;

public class FormatadoresTests
{
    [Trait("Solucao", "Test")]
    [Trait("Acao", "Formatadores")]
    [Theory(DisplayName = "Dado minutos, quando MinutosParaHoras, então formata HHhMM")]
    [InlineData(0, "—")]
    [InlineData(75, "01h15")]
    [InlineData(480, "08h00")]
    public void MinutosParaHoras_Formata(int min, string esperado)
        => Formatadores.MinutosParaHoras(min).Should().Be(esperado);

    [Trait("Solucao", "Test")]
    [Trait("Acao", "Formatadores")]
    [Theory(DisplayName = "Dado saldo, quando MinutosParaHorasComSinal, então prefixa + ou -")]
    [InlineData(0, "0h00")]
    [InlineData(45, "+00h45")]
    [InlineData(-30, "-00h30")]
    public void MinutosParaHorasComSinal_Formata(int min, string esperado)
        => Formatadores.MinutosParaHorasComSinal(min).Should().Be(esperado);

    [Trait("Solucao", "Test")]
    [Trait("Acao", "Formatadores")]
    [Fact(DisplayName = "Dado CPF 11 dígitos, quando FormatarCpf, então retorna mascarado")]
    public void FormatarCpf_Mascara()
        => Formatadores.FormatarCpf("12345678900").Should().Be("123.456.789-00");
}
