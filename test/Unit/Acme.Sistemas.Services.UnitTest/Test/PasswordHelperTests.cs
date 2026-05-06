using Acme.Sistemas.Core.Security;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class PasswordHelperTests
{
    [Fact]
    public void Hash_e_Verify_DevemReconhecerSenhaCorreta()
    {
        var password = "Atena@2026!";
        var stored = PasswordHelper.Hash(password);

        PasswordHelper.Verify(password, stored).Should().BeTrue();
    }

    [Fact]
    public void Verify_DeveRejeitarSenhaIncorreta()
    {
        var stored = PasswordHelper.Hash("CorretaSenha@1");

        PasswordHelper.Verify("OutraSenha@1", stored).Should().BeFalse();
    }

    [Theory]
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
