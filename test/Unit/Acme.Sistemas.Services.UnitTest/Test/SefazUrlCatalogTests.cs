using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.ExternalIntegration.Sefaz.Urls;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class SefazUrlCatalogTests
{
    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "SefazUrlCatalog")]
    [Theory(DisplayName = "Dado as 5 UFs prioritárias + autorizadoras especiais, quando Resolver autorização homolog, então retorna URL conhecida e https")]
    [InlineData("SP", "homologacao.nfe.fazenda.sp.gov.br")]
    [InlineData("RJ", "nfe-homologacao.svrs.rs.gov.br")]
    [InlineData("MG", "hnfe.fazenda.mg.gov.br")]
    [InlineData("RS", "nfe-homologacao.svrs.rs.gov.br")]
    [InlineData("PR", "homologacao.nfe.sefa.pr.gov.br")]
    [InlineData("SVRS", "nfe-homologacao.svrs.rs.gov.br")]
    [InlineData("SVAN", "hom.sefazvirtual.fazenda.gov.br")]
    public void Resolver_UFsPrioritarias_RetornaUrlComHostEsperado(string uf, string hostEsperado)
    {
        var sut = new SefazUrlCatalog();

        var url = sut.GetAutorizacao(uf, AmbienteFiscal.Homologacao);

        url.Should().StartWith("https://").And.Contain(hostEsperado);
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "SefazUrlCatalog")]
    [Fact(DisplayName = "Dado uma UF não-cobrirada, quando Resolver, então lança KeyNotFoundException listando UFs disponíveis")]
    public void Resolver_UFInexistente_LancaComListaDeDisponiveis()
    {
        var sut = new SefazUrlCatalog();

        Action act = () => sut.GetAutorizacao("XX", AmbienteFiscal.Homologacao);

        act.Should().Throw<KeyNotFoundException>().WithMessage("*UF 'XX'*disponíveis*SP*RJ*");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "SefazUrlCatalog")]
    [Fact(DisplayName = "Dado override definido, quando Resolver mesma combinação, então retorna URL do override (ignora catálogo)")]
    public void Resolver_ComOverride_PrecedenciaSobreEmbarcado()
    {
        var sut = new SefazUrlCatalog();
        sut.DefinirOverride("SP", AmbienteFiscal.Homologacao, SefazServico.Autorizacao, "https://mock-sefaz.local/autorizacao");

        sut.GetAutorizacao("SP", AmbienteFiscal.Homologacao).Should().Be("https://mock-sefaz.local/autorizacao");
        // Outros serviços da mesma UF/ambiente continuam vindo do catálogo
        sut.GetStatusServico("SP", AmbienteFiscal.Homologacao).Should().Contain("homologacao.nfe.fazenda.sp.gov.br");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "SefazUrlCatalog")]
    [Fact(DisplayName = "Dado o catálogo carregado, quando consultar UFsDisponiveis, então inclui as 5 prioritárias + SVRS + SVAN")]
    public void UFsDisponiveis_Inclui5PrioritariasMaisAutorizadorasEspeciais()
    {
        var sut = new SefazUrlCatalog();

        sut.UFsDisponiveis.Should().Contain(new[] { "SP", "RJ", "MG", "RS", "PR", "SVRS", "SVAN" });
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "SefazUrlCatalog")]
    [Fact(DisplayName = "Dado SP em ambos ambientes, quando Resolver homolog vs producao, então URLs são distintas e ambas https")]
    public void Resolver_HomologVsProducao_UrlsDistintas()
    {
        var sut = new SefazUrlCatalog();

        var hom = sut.GetAutorizacao("SP", AmbienteFiscal.Homologacao);
        var prod = sut.GetAutorizacao("SP", AmbienteFiscal.Producao);

        hom.Should().NotBe(prod);
        hom.Should().Contain("homologacao");
        prod.Should().NotContain("homologacao");
    }
}
