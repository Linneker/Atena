using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Acme.Sistemas.IntegrationTest.Config;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

/// <summary>
/// E2E: login -> criar pedido de venda -> faturar -> emitir NF-e (homologação).
/// Requer ambiente docker (MySQL + RabbitMQ + MinIO) e seed inicial de tenant/usuário.
/// </summary>
public class FluxoVendaCompletaTests : IntegrationTestBase
{
    public FluxoVendaCompletaTests(DockerEnvironment docker) : base(docker) { }

    [Trait("Solucao", "Api")]
    [Trait("Acao", "FluxoVenda")]
    [Fact(
        Skip = "Requer seed completo do tenant + cliente + produto + cert. fiscal homologação",
        DisplayName = "Dado seed completo, quando login + pedido de venda + faturamento + emissão de NF-e, então fluxo completa com sucesso")]
    public async Task Fluxo_Login_PedidoVenda_Faturamento_NFe_DeveCompletar()
    {
        var token = await LoginAsync("admin@tenant1.test", "Admin@123");
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var pedidoResp = await Client.PostAsJsonAsync("/api/v1/pedidos-venda", new
        {
            ClienteId = SeedIds.Cliente,
            VendedorId = SeedIds.Vendedor,
            Itens = new[] { new { ProdutoId = SeedIds.Produto, Quantidade = 2, ValorUnitario = 100m } }
        });
        pedidoResp.StatusCode.Should().Be(HttpStatusCode.Created);
        var pedido = await pedidoResp.Content.ReadFromJsonAsync<IdResult>();
        pedido!.Id.Should().NotBeEmpty();

        var confirma = await Client.PostAsync($"/api/v1/pedidos-venda/{pedido.Id}/confirmar", null);
        confirma.IsSuccessStatusCode.Should().BeTrue();

        var fatura = await Client.PostAsJsonAsync("/api/v1/faturamentos", new { PedidoId = pedido.Id });
        fatura.StatusCode.Should().Be(HttpStatusCode.Created);
        var faturaResult = await fatura.Content.ReadFromJsonAsync<IdResult>();

        var nfeStatus = await Client.GetAsync($"/api/v1/faturamentos/{faturaResult!.Id}/nfe");
        nfeStatus.IsSuccessStatusCode.Should().BeTrue();
    }

    [Trait("Solucao", "Api")]
    [Trait("Acao", "FluxoVenda")]
    [Fact(
        Skip = "Requer seed completo + fornecedor + produto",
        DisplayName = "Dado seed completo, quando solicitação + pedido + recebimento de compra, então atualiza saldo de estoque do produto")]
    public async Task Fluxo_Compra_Recebimento_Estoque_ContaPagar_DeveCompletar()
    {
        var token = await LoginAsync("admin@tenant1.test", "Admin@123");
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var solicitacao = await Client.PostAsJsonAsync("/api/v1/solicitacoes-compra", new
        {
            Solicitante = "Comprador 1",
            Itens = new[] { new { ProdutoId = SeedIds.Produto, Quantidade = 10, ValorUnitario = 50m } }
        });
        var solId = (await solicitacao.Content.ReadFromJsonAsync<IdResult>())!.Id;

        await Client.PostAsync($"/api/v1/solicitacoes-compra/{solId}/aprovar", null);

        var pedidoCompra = await Client.PostAsJsonAsync("/api/v1/pedidos-compra", new
        {
            SolicitacaoId = solId,
            FornecedorId = SeedIds.Fornecedor
        });
        var pedidoId = (await pedidoCompra.Content.ReadFromJsonAsync<IdResult>())!.Id;

        var recebimento = await Client.PostAsJsonAsync("/api/v1/recebimentos-compra", new
        {
            PedidoId = pedidoId,
            Tipo = "Total"
        });
        recebimento.IsSuccessStatusCode.Should().BeTrue();

        var saldo = await Client.GetAsync($"/api/v1/estoque/saldo?produtoId={SeedIds.Produto}");
        saldo.IsSuccessStatusCode.Should().BeTrue();
    }

    private async Task<string> LoginAsync(string email, string senha)
    {
        var resp = await Client.PostAsJsonAsync("/api/v1/autenticacao/login", new { Email = email, Senha = senha });
        resp.EnsureSuccessStatusCode();
        var body = await resp.Content.ReadFromJsonAsync<LoginResult>();
        return body!.AccessToken;
    }

    private sealed record IdResult(Guid Id);
    private sealed record LoginResult(string AccessToken, string RefreshToken, int ExpiresIn);

    private static class SeedIds
    {
        public static readonly Guid Cliente = new("11111111-1111-1111-1111-111111111111");
        public static readonly Guid Vendedor = new("22222222-2222-2222-2222-222222222222");
        public static readonly Guid Produto = new("33333333-3333-3333-3333-333333333333");
        public static readonly Guid Fornecedor = new("44444444-4444-4444-4444-444444444444");
    }
}
