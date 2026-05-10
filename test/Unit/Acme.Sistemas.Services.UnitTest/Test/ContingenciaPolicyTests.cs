using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.ExternalIntegration.Sefaz.Contingencia;
using Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class ContingenciaPolicyTests
{
    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "ContingenciaPolicy")]
    [Fact(DisplayName = "Dado estado inicial limpo, quando UfParaUsar, então retorna a UF original (sem contingência)")]
    public void UfParaUsar_SemEstado_RetornaUfOriginal()
    {
        var sut = new ContingenciaPolicy();
        sut.UfParaUsar("SP", AmbienteFiscal.Homologacao).Should().Be("SP");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "ContingenciaPolicy")]
    [Fact(DisplayName = "Dado erro de rede registrado, quando UfParaUsar dentro da janela, então retorna SVRS")]
    public void UfParaUsar_ErroDeRede_VaiParaSVRS()
    {
        var sut = new ContingenciaPolicy(janelaIndisponibilidade: TimeSpan.FromMinutes(5));

        sut.RegistrarRespostaTransmissao("SP", AmbienteFiscal.Homologacao, cStat: null, motivo: "timeout", erroDeRede: true);

        sut.UfParaUsar("SP", AmbienteFiscal.Homologacao).Should().Be("SVRS");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "ContingenciaPolicy")]
    [Theory(DisplayName = "Dado cStat de paralisação SEFAZ (108/109), quando RegistrarRespostaTransmissao, então marca indisponível e roteia para SVRS")]
    [InlineData("108")]
    [InlineData("109")]
    public void UfParaUsar_CStatParalisacao_VaiParaSVRS(string cStat)
    {
        var sut = new ContingenciaPolicy();

        sut.RegistrarRespostaTransmissao("RJ", AmbienteFiscal.Producao, cStat, "paralisado", erroDeRede: false);

        sut.UfParaUsar("RJ", AmbienteFiscal.Producao).Should().Be("SVRS");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "ContingenciaPolicy")]
    [Fact(DisplayName = "Dado cStat=107 (operando), quando RegistrarRespostaStatusServico, então limpa estado e UfParaUsar volta para origem")]
    public void RegistrarRespostaStatusServico_Operando_LimpaEstado()
    {
        var sut = new ContingenciaPolicy();
        sut.RegistrarRespostaTransmissao("SP", AmbienteFiscal.Homologacao, "108", "paralisado", erroDeRede: false);
        sut.UfParaUsar("SP", AmbienteFiscal.Homologacao).Should().Be("SVRS");

        var resultado = new StatusServicoResultado("107", "Servico em operacao", Operando: true, Paralisado: false, ConsultadoEm: DateTime.UtcNow, RetornoXml: null);
        sut.RegistrarRespostaStatusServico("SP", AmbienteFiscal.Homologacao, resultado);

        sut.UfParaUsar("SP", AmbienteFiscal.Homologacao).Should().Be("SP");
        sut.GetEstado("SP", AmbienteFiscal.Homologacao).Should().BeNull();
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "ContingenciaPolicy")]
    [Fact(DisplayName = "Dado janela de indisponibilidade expirada, quando UfParaUsar, então volta para origem (modo tentar de novo)")]
    public async Task UfParaUsar_AposJanelaExpirar_VoltaParaOrigem()
    {
        var sut = new ContingenciaPolicy(janelaIndisponibilidade: TimeSpan.FromMilliseconds(50));
        sut.RegistrarRespostaTransmissao("MG", AmbienteFiscal.Homologacao, null, "timeout", erroDeRede: true);
        sut.UfParaUsar("MG", AmbienteFiscal.Homologacao).Should().Be("SVRS");

        await Task.Delay(100);

        sut.UfParaUsar("MG", AmbienteFiscal.Homologacao).Should().Be("MG");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "ContingenciaPolicy")]
    [Fact(DisplayName = "Dado ForcarContingencia manual, quando UfParaUsar, então roteia para SVRS independente de cStat ou rede")]
    public void ForcarContingencia_Manual_RoteiaParaSVRS()
    {
        var sut = new ContingenciaPolicy();

        sut.ForcarContingencia("PR", AmbienteFiscal.Homologacao, "Manutencao programada interna");

        sut.UfParaUsar("PR", AmbienteFiscal.Homologacao).Should().Be("SVRS");
        sut.GetEstado("PR", AmbienteFiscal.Homologacao)!.UltimoMotivo.Should().Be("Manutencao programada interna");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "ContingenciaPolicy")]
    [Fact(DisplayName = "Dado contingência ativa, quando LimparContingencia manual, então UfParaUsar volta para origem imediatamente")]
    public void LimparContingencia_Manual_VoltaParaOrigem()
    {
        var sut = new ContingenciaPolicy();
        sut.ForcarContingencia("RS", AmbienteFiscal.Producao, "teste");
        sut.UfParaUsar("RS", AmbienteFiscal.Producao).Should().Be("SVRS");

        sut.LimparContingencia("RS", AmbienteFiscal.Producao);

        sut.UfParaUsar("RS", AmbienteFiscal.Producao).Should().Be("RS");
    }
}
