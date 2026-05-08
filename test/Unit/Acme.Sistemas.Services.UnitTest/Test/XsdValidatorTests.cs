using Acme.Sistemas.ExternalIntegration.Sefaz;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class XsdValidatorTests
{
    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "XsdValidator")]
    [Fact(DisplayName = "Dado XSDs ainda não embarcados (apenas README), quando Validar, então lança InvalidOperationException com instrução clara")]
    public void Validar_SemSchemasEmbarcados_LancaComMensagemClara()
    {
        var sut = new XsdValidator();
        Action act = () => sut.Validar("<x/>");

        // Enquanto os XSDs oficiais não forem adicionados (task 1.1.3 BLOQUEADO),
        // o validator falha rápido com uma mensagem direcionando ao README.
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Schemas XSD NFe v4.00 não encontrados*README.md*");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "XsdValidator")]
    [Fact(DisplayName = "Dado o estado atual do projeto, quando TemSchemasCarregados, então retorna false (XSDs ainda pendentes — task 1.1.3)")]
    public void TemSchemasCarregados_AindaSemXsds_RetornaFalse()
    {
        XsdValidator.TemSchemasCarregados().Should().BeFalse();
    }
}
