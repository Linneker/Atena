using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Acme.Sistemas.IntegrationTest.Config;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

/// <summary>
/// Garante que dados de um tenant não são acessíveis por outro.
/// Cria recurso com Tenant A, tenta ler/alterar/excluir com token de Tenant B.
/// </summary>
public class IsolamentoCrossTenantTests : IntegrationTestBase
{
    public IsolamentoCrossTenantTests(DockerEnvironment docker) : base(docker) { }

    [Fact(Skip = "Requer seed de dois tenants distintos no ambiente de teste")]
    public async Task TenantB_NaoAcessaCliente_CriadoPorTenantA()
    {
        var tokenA = await LoginAsync("admin@tenant-a.test", "Admin@123");
        var tokenB = await LoginAsync("admin@tenant-b.test", "Admin@123");

        SetToken(tokenA);
        var criar = await Client.PostAsJsonAsync("/api/v1/clientes", new
        {
            Tipo = "Fisica",
            Nome = "Cliente Tenant A",
            Documento = "111.111.111-11"
        });
        criar.StatusCode.Should().Be(HttpStatusCode.Created);
        var id = (await criar.Content.ReadFromJsonAsync<IdResult>())!.Id;

        SetToken(tokenB);
        var ler = await Client.GetAsync($"/api/v1/clientes/{id}");
        ler.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.Forbidden);

        var atualizar = await Client.PutAsJsonAsync($"/api/v1/clientes/{id}", new { Nome = "Hacker" });
        atualizar.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.Forbidden);

        var excluir = await Client.DeleteAsync($"/api/v1/clientes/{id}");
        excluir.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.Forbidden);

        SetToken(tokenA);
        var listagem = await Client.GetAsync("/api/v1/clientes");
        listagem.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact(Skip = "Requer seed de dois tenants e produtos em cada um")]
    public async Task Listagem_TenantB_NaoRetornaProdutos_DeTenantA()
    {
        var tokenA = await LoginAsync("admin@tenant-a.test", "Admin@123");
        var tokenB = await LoginAsync("admin@tenant-b.test", "Admin@123");

        SetToken(tokenA);
        var pa = await Client.PostAsJsonAsync("/api/v1/produtos", new { Codigo = "A-001", Descricao = "Produto Tenant A" });
        var idA = (await pa.Content.ReadFromJsonAsync<IdResult>())!.Id;

        SetToken(tokenB);
        var lista = await Client.GetFromJsonAsync<PaginaResultado<ProdutoDto>>("/api/v1/produtos?pagina=1&tamanhoPagina=100");
        lista!.Itens.Should().NotContain(p => p.Id == idA);
    }

    private void SetToken(string token) =>
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    private async Task<string> LoginAsync(string email, string senha)
    {
        var resp = await Client.PostAsJsonAsync("/api/v1/autenticacao/login", new { Email = email, Senha = senha });
        resp.EnsureSuccessStatusCode();
        var body = await resp.Content.ReadFromJsonAsync<LoginResult>();
        return body!.AccessToken;
    }

    private sealed record IdResult(Guid Id);
    private sealed record LoginResult(string AccessToken, string RefreshToken, int ExpiresIn);
    private sealed record ProdutoDto(Guid Id, string Codigo, string Descricao);
    private sealed record PaginaResultado<T>(IReadOnlyList<T> Itens, int Total, int Pagina, int TamanhoPagina);
}
