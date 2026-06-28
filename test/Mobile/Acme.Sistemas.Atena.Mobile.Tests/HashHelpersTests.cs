using Acme.Sistemas.Atena.Mobile.Shared.Helpers;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Atena.Mobile.Tests;

public class HashHelpersTests
{
    [Trait("Solucao", "Test")]
    [Trait("Acao", "HashHelpers")]
    [Fact(DisplayName = "Dado mesmos campos, quando CalcularHashBatida, então hash é determinístico")]
    public void HashBatida_Determinista()
    {
        var ts = new DateTime(2026, 6, 28, 12, 0, 0, DateTimeKind.Utc);
        var h1 = HashHelpers.CalcularHashBatida("func-1", ts, "Entrada", "dev-x");
        var h2 = HashHelpers.CalcularHashBatida("func-1", ts, "Entrada", "dev-x");
        h1.Should().Be(h2);
        h1.Should().HaveLength(64);
    }

    [Trait("Solucao", "Test")]
    [Trait("Acao", "HashHelpers")]
    [Fact(DisplayName = "Dado deviceId diferente, quando CalcularHashBatida, então hash muda")]
    public void HashBatida_DeviceIdDistinto_HashDiferente()
    {
        var ts = new DateTime(2026, 6, 28, 12, 0, 0, DateTimeKind.Utc);
        var h1 = HashHelpers.CalcularHashBatida("func-1", ts, "Entrada", "dev-A");
        var h2 = HashHelpers.CalcularHashBatida("func-1", ts, "Entrada", "dev-B");
        h1.Should().NotBe(h2);
    }

    [Trait("Solucao", "Test")]
    [Trait("Acao", "HashHelpers")]
    [Fact(DisplayName = "Dado input vazio, quando Sha256Hex, então retorna SHA-256 em hex 64 chars")]
    public void Sha256Hex_VazioConhecido()
    {
        HashHelpers.Sha256Hex(string.Empty).Should()
            .Be("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");
    }
}
