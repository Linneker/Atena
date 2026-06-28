using Acme.Sistemas.Core.Helper;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

/// <summary>
/// Cobertura dos validators brasileiros usados por RH/Folha: PIS/PASEP (DV mod 11),
/// CTPS (formato + UF) e conta bancária (banco/agência/conta com DVs). Cada validador
/// é puro/estático — testes sem dependências externas.
/// </summary>
public class ValidadoresBrasileirosTests
{
    // ============================================================ PIS/PASEP

    [Trait("Solucao", "Core")]
    [Trait("Acao", "PisHelper")]
    [Theory(DisplayName = "Dado PIS válido conhecido (DV calculado pelo algoritmo oficial mod 11), quando IsValid, então retorna true")]
    [InlineData("12345678900")]   // sum=231, rem=0, DV=0 ✓
    [InlineData("12056412820")]   // sum=154, rem=0, DV=0 ✓
    [InlineData("10000000008")]   // sum=1*3=3, rem=3, DV=8 ✓
    public void PisHelper_PisValido_RetornaTrue(string pis)
    {
        PisHelper.IsValid(pis).Should().BeTrue();
    }

    [Trait("Solucao", "Core")]
    [Trait("Acao", "PisHelper")]
    [Theory(DisplayName = "Dado PIS inválido (DV errado, repetido, vazio ou tamanho), quando IsValid, então retorna false")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("00000000000")]   // todos zeros
    [InlineData("11111111111")]   // sequência repetida
    [InlineData("12345678901")]   // DV trocado (correto: 0)
    [InlineData("12345678905")]   // DV trocado (correto: 0)
    [InlineData("1205641282")]    // 10 dígitos
    [InlineData("abcdefghijk")]   // sem dígitos
    public void PisHelper_PisInvalido_RetornaFalse(string? pis)
    {
        PisHelper.IsValid(pis!).Should().BeFalse();
    }

    [Trait("Solucao", "Core")]
    [Trait("Acao", "PisHelper")]
    [Fact(DisplayName = "Dado PIS válido com pontuação, quando IsValid, então normaliza dígitos e aceita")]
    public void PisHelper_AceitaPontuacao()
    {
        PisHelper.IsValid("123.45678.90-0").Should().BeTrue();
    }

    // ============================================================ CTPS

    [Trait("Solucao", "Core")]
    [Trait("Acao", "CtpsHelper")]
    [Fact(DisplayName = "Dado CTPS com número, série e UF válidos, quando IsValid, então retorna true")]
    public void CtpsHelper_FormatoValido_RetornaTrue()
    {
        CtpsHelper.IsValid("1234567", "001", "SP").Should().BeTrue();
        CtpsHelper.IsValid("12345678", "012", "RJ").Should().BeTrue();
    }

    [Trait("Solucao", "Core")]
    [Trait("Acao", "CtpsHelper")]
    [Theory(DisplayName = "Dado UF inválida ou campos faltantes, quando IsValid, então retorna false")]
    [InlineData("1234567", "001", "XX")]   // UF não existe
    [InlineData("123", "001", "SP")]        // número curto demais
    [InlineData("123456789", "001", "SP")]  // número longo demais
    [InlineData("1234567", "0", "SP")]      // série curta demais
    [InlineData("", "001", "SP")]
    [InlineData("1234567", "", "SP")]
    [InlineData("1234567", "001", "")]
    public void CtpsHelper_FormatoInvalido_RetornaFalse(string num, string serie, string uf)
    {
        CtpsHelper.IsValid(num, serie, uf).Should().BeFalse();
    }

    // ============================================================ Conta Bancária

    [Trait("Solucao", "Core")]
    [Trait("Acao", "ContaBancariaHelper")]
    [Fact(DisplayName = "Dado banco/agência/conta com formatos corretos, quando IsValid, então retorna true")]
    public void ContaBancaria_FormatosValidos_RetornaTrue()
    {
        // Itaú: 341, agência 0001 DV 9, conta 12345 DV 6
        ContaBancariaHelper.IsValid("341", "0001", "9", "12345", "6").Should().BeTrue();
        // Conta com DV 'X' (alguns bancos)
        ContaBancariaHelper.IsValid("237", "1234", "5", "98765", "X").Should().BeTrue();
    }

    [Trait("Solucao", "Core")]
    [Trait("Acao", "ContaBancariaHelper")]
    [Theory(DisplayName = "Dado dados de conta bancária inválidos (banco, agência ou conta fora do formato), quando IsValid, então retorna false")]
    [InlineData("00", "0001", "9", "12345", "6")]     // banco curto
    [InlineData("000", "0001", "9", "12345", "6")]    // banco "000" não existe
    [InlineData("341", "12", "9", "12345", "6")]      // agência curta
    [InlineData("341", "0001", "9", "12", "6")]       // conta curta
    [InlineData("341", "0001", "9", "12345", "")]     // sem DV
    [InlineData("341", "0001", "9", "12345", "AB")]   // DV com mais de 1 char
    public void ContaBancaria_FormatosInvalidos_RetornaFalse(
        string banco, string ag, string agDv, string conta, string contaDv)
    {
        ContaBancariaHelper.IsValid(banco, ag, agDv, conta, contaDv).Should().BeFalse();
    }
}
