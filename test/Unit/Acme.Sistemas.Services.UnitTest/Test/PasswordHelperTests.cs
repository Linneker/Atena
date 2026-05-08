using Acme.Sistemas.Core.Security;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class PasswordHelperTests
{
    [Trait("Solucao", "Core")]
    [Trait("Acao", "PasswordHelper")]
    [Fact(DisplayName = "Dado uma senha hashada, quando Verify com a mesma senha, então retorna true")]
    public void Hash_e_Verify_DevemReconhecerSenhaCorreta()
    {
        var password = "Atena@2026!";
        var stored = PasswordHelper.Hash(password);

        PasswordHelper.Verify(password, stored).Should().BeTrue();
    }

    [Trait("Solucao", "Core")]
    [Trait("Acao", "PasswordHelper")]
    [Fact(DisplayName = "Dado uma senha hashada, quando Verify com senha diferente, então retorna false")]
    public void Verify_DeveRejeitarSenhaIncorreta()
    {
        var stored = PasswordHelper.Hash("CorretaSenha@1");

        PasswordHelper.Verify("OutraSenha@1", stored).Should().BeFalse();
    }

    [Trait("Solucao", "Core")]
    [Trait("Acao", "PasswordHelper")]
    [Theory(DisplayName = "Dado uma senha, quando IsStrong, então valida regras de minúscula, maiúscula, dígito, especial e tamanho")]
    [InlineData("Atena@2026", true)]
    [InlineData("atena2026", false)]
    [InlineData("ATENA2026", false)]
    [InlineData("Atena2026", false)]
    [InlineData("Atn@1", false)]
    public void IsStrong_DeveValidarRegrasDeSenhaForte(string password, bool expected)
    {
        PasswordHelper.IsStrong(password).Should().Be(expected);
    }
}
