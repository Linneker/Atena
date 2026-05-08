using Acme.Sistemas.Domain.Entities.Fiscal.Xml;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class ChaveAcessoBuilderTests
{
    [Trait("Solucao", "Domain")]
    [Trait("Acao", "ChaveAcessoBuilder")]
    [Fact(DisplayName = "Dado componentes válidos, quando Build, então monta chave de 44 dígitos com DV correto no final")]
    public void Build_ComponentesValidos_GeraChaveDe44DigitosComDV()
    {
        var chave = ChaveAcessoBuilder.Build(
            cUF: "35",                              // SP
            dataEmissao: new DateTime(2026, 5, 8),  // AAMM = 2605
            cnpj: "12345678000199",
            modelo: "55",
            serie: "1",
            numero: 7,
            tpEmis: 1,
            cNF: "11908850");

        chave.Should().HaveLength(44);
        // Dígitos parciais (43): cUF(35) + AAMM(2605) + CNPJ(14) + mod(55) + serie(001) + nNF(000000007) + tpEmis(1) + cNF(11908850)
        chave[..43].Should().Be("3526051234567800019955001000000007111908850");
        ChaveAcessoBuilder.CalcularDV(chave[..43]).Should().Be(chave[43]);
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "ChaveAcessoBuilder")]
    [Theory(DisplayName = "Dado chaves de 43 dígitos com somas conhecidas, quando CalcularDV, então retorna dígito mod 11 esperado (incluindo casos de resto 0 e 1 → DV 0)")]
    // 43 zeros: soma = 0, resto = 0 → DV = 0
    [InlineData("0000000000000000000000000000000000000000000", '0')]
    // 43 uns: soma de pesos cíclicos 2..9 ao longo de 43 posições = 5*44 + (2+3+4) = 220+9 = 229
    //   229 mod 11 = 9 → DV = 11 - 9 = 2
    [InlineData("1111111111111111111111111111111111111111111", '2')]
    public void CalcularDV_CasosConhecidos(string chave43, char dvEsperado)
    {
        ChaveAcessoBuilder.CalcularDV(chave43).Should().Be(dvEsperado);
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "ChaveAcessoBuilder")]
    [Fact(DisplayName = "Dado série e número curtos, quando Build, então faz zero-padding para 3 e 9 dígitos")]
    public void Build_PadronizaSerieENumeroComZerosAEsquerda()
    {
        var chave = ChaveAcessoBuilder.Build(
            cUF: "33", dataEmissao: new DateTime(2026, 1, 1),
            cnpj: "11222333000144", modelo: "55", serie: "1", numero: 1, tpEmis: 1,
            cNF: "00000001");

        // Offsets: cUF(0..1) + AAMM(2..5) + CNPJ(6..19) + mod(20..21) + serie(22..24) + nNF(25..33) + tpEmis(34) + cNF(35..42)
        chave.Substring(20, 5).Should().Be("55001");           // mod(2) + serie(3)
        chave.Substring(25, 9).Should().Be("000000001");        // nNF(9)
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "ChaveAcessoBuilder")]
    [Fact(DisplayName = "Dado mesmo conjunto de componentes, quando Build duas vezes, então gera a mesma chave (determinístico)")]
    public void Build_Deterministico_MesmaChaveParaMesmosComponentes()
    {
        var args = new object[]
        {
            "35", new DateTime(2026, 5, 8), "12345678000199", "55", "1", 42L, 1, "00042001"
        };

        var chave1 = ChaveAcessoBuilder.Build("35", new DateTime(2026, 5, 8), "12345678000199", "55", "1", 42, 1, "00042001");
        var chave2 = ChaveAcessoBuilder.Build("35", new DateTime(2026, 5, 8), "12345678000199", "55", "1", 42, 1, "00042001");
        chave1.Should().Be(chave2);
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "ChaveAcessoBuilder")]
    [Fact(DisplayName = "Dado CNPJ com tamanho inválido, quando Build, então lança ArgumentException com mensagem clara")]
    public void Build_CnpjInvalido_LancaArgumentException()
    {
        Action act = () => ChaveAcessoBuilder.Build(
            cUF: "35", dataEmissao: DateTime.UtcNow, cnpj: "123",
            modelo: "55", serie: "1", numero: 1, tpEmis: 1, cNF: "00000001");

        act.Should().Throw<ArgumentException>().WithMessage("CNPJ*");
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "ChaveAcessoBuilder")]
    [Fact(DisplayName = "Dado o último dígito da chave gerada, quando comparado ao CalcularDV dos primeiros 43, então são iguais (auto-consistência)")]
    public void Build_CDvDaChave_BateComCalcularDV()
    {
        // Roda 50 chaves diferentes e garante que o último dígito sempre é o DV correto
        var rng = new Random(42);
        for (var i = 0; i < 50; i++)
        {
            var chave = ChaveAcessoBuilder.Build(
                cUF: rng.Next(11, 53).ToString().PadLeft(2, '0'),
                dataEmissao: new DateTime(2024 + rng.Next(0, 4), rng.Next(1, 13), rng.Next(1, 28)),
                cnpj: rng.NextInt64(10_000_000_000_000L, 99_999_999_999_999L).ToString(),
                modelo: "55",
                serie: rng.Next(1, 999).ToString(),
                numero: rng.NextInt64(1, 999_999_999),
                tpEmis: rng.Next(1, 8),
                cNF: rng.Next(0, 100_000_000).ToString().PadLeft(8, '0'));

            chave[43].Should().Be(ChaveAcessoBuilder.CalcularDV(chave[..43]),
                because: $"chave gerada deveria ter DV consistente: {chave}");
        }
    }
}
