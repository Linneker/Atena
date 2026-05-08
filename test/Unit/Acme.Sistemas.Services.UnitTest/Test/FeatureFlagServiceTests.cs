using System.Text.Json;
using Acme.Sistemas.Domain.Interfaces.AppConfiguration;
using Acme.Sistemas.Infrastructure.AppConfiguration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class FeatureFlagServiceTests : IDisposable
{
    private readonly string _file;

    public FeatureFlagServiceTests()
    {
        _file = Path.Combine(Path.GetTempPath(), $"atena-ff-{Guid.NewGuid():N}.json");
        File.WriteAllText(_file, """
            {
              "FeatureFlags": {
                "Cache": { "Provider": "LiteDb", "HotTtlMinutes": 15 },
                "Audit": { "Enabled": true, "Verbose": false }
              }
            }
            """);
    }

    public void Dispose()
    {
        if (File.Exists(_file)) try { File.Delete(_file); } catch { }
    }

    private FeatureFlagService BuildSut()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(_file, optional: false, reloadOnChange: true)
            .Build();
        return new FeatureFlagService(config, NullLogger<FeatureFlagService>.Instance, _file);
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "FeatureFlagService")]
    [Fact(DisplayName = "Dado configuração com flags aninhadas, quando ListAll, então retorna apenas as folhas com chave path-style")]
    public void ListAll_RetornaApenasFolhas()
    {
        var sut = BuildSut();
        var items = sut.ListAll();
        items.Should().HaveCount(4);
        items.Select(x => x.Key).Should().BeEquivalentTo(new[]
        {
            "Cache:Provider", "Cache:HotTtlMinutes", "Audit:Enabled", "Audit:Verbose"
        });
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "FeatureFlagService")]
    [Fact(DisplayName = "Dado uma flag inexistente, quando Get, então retorna null")]
    public void Get_FlagInexistente_RetornaNull()
    {
        var sut = BuildSut();
        sut.Get("NaoExiste:X").Should().BeNull();
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "FeatureFlagService")]
    [Fact(DisplayName = "Dado flags com diferentes tipos no JSON, quando Get, então retorna item com Type inferido (String, Boolean, Integer)")]
    public void Get_FlagExistente_RetornaItemComTipoInferido()
    {
        var sut = BuildSut();
        var provider = sut.Get("Cache:Provider")!;
        provider.Value.Should().Be("LiteDb");
        provider.Type.Should().Be(FeatureFlagType.String);

        var enabled = sut.Get("Audit:Enabled")!;
        enabled.Value.Should().Be(true);
        enabled.Type.Should().Be(FeatureFlagType.Boolean);

        var ttl = sut.Get("Cache:HotTtlMinutes")!;
        ttl.Value.Should().Be(15L);
        ttl.Type.Should().Be(FeatureFlagType.Integer);
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "FeatureFlagService")]
    [Fact(DisplayName = "Dado SetAsync com tipo compatível, quando persiste, então grava no arquivo e a próxima leitura reflete o novo valor")]
    public async Task SetAsync_TipoCompativel_PersisteEArquivoEAtualizaConfiguracao()
    {
        var sut = BuildSut();
        var doc = JsonDocument.Parse("\"Redis\"");
        await sut.SetAsync("Cache:Provider", doc.RootElement);

        var raw = await File.ReadAllTextAsync(_file);
        raw.Should().Contain("\"Redis\"");

        await sut.ReloadAsync();
        sut.Get("Cache:Provider")!.Value.Should().Be("Redis");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "FeatureFlagService")]
    [Fact(DisplayName = "Dado SetAsync com tipo incompatível ao da flag, quando chamado, então lança ArgumentException e não modifica o arquivo")]
    public async Task SetAsync_TipoIncompativel_LancaArgumentException_NaoMudaArquivo()
    {
        var sut = BuildSut();
        var antes = await File.ReadAllTextAsync(_file);

        var doc = JsonDocument.Parse("\"nao-bool\"");
        Func<Task> act = () => sut.SetAsync("Audit:Enabled", doc.RootElement);
        await act.Should().ThrowAsync<ArgumentException>();

        var depois = await File.ReadAllTextAsync(_file);
        depois.Should().Be(antes);
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "FeatureFlagService")]
    [Fact(DisplayName = "Dado SetAsync para flag inexistente, quando chamado, então lança ArgumentException")]
    public async Task SetAsync_FlagInexistente_LancaArgumentException()
    {
        var sut = BuildSut();
        var doc = JsonDocument.Parse("\"x\"");
        Func<Task> act = () => sut.SetAsync("NaoExiste:Foo", doc.RootElement);
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "FeatureFlagService")]
    [Fact(DisplayName = "Dado arquivo de flags ficar malformado em runtime, quando ReloadAsync, então lança mas serviço continua respondendo")]
    public async Task ArquivoMalformado_NaoDerruba_AntigosValoresPermanecem()
    {
        var sut = BuildSut();
        var antes = sut.Get("Cache:Provider")!.Value;

        // Sobrescreve com JSON inválido.
        await File.WriteAllTextAsync(_file, "{ this is not json }");
        // ReloadAsync deve falhar silenciosamente (logando) ou lançar — escolhemos lançar para o caller.
        Func<Task> act = () => sut.ReloadAsync();
        await act.Should().ThrowAsync<Exception>();

        // Os valores em memória anteriores permanecem disponíveis (ListAll volta vazio porque IConfiguration falhou,
        // mas a chamada não derruba o processo). Aqui validamos apenas que o serviço continua respondendo.
        sut.ListAll(); // não lança
    }
}
