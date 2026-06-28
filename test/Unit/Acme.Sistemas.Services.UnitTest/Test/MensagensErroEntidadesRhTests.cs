using System.Text.Json;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

/// <summary>
/// Cobertura das mensagens de erro nos caminhos negativos das entidades RH:
/// (a) desserialização de JSON malformado nos value objects que vão para colunas JSON
/// (endereco/conta) deve produzir <see cref="JsonException"/> com path e posição;
/// (b) <see cref="Enum.Parse(Type, string)"/> em valor desconhecido deve produzir
/// <see cref="ArgumentException"/> citando o valor recusado — protege contra catálogos
/// (UI/CSV de importação) que confiem em strings opacas.
/// </summary>
public class MensagensErroEntidadesRhTests
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    // --------------------------------------------- JSON malformado em value objects

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "EnderecoFuncionario")]
    [Fact(DisplayName = "Dado JSON com chave mal-formada, quando desserializa EnderecoFuncionario, então JsonException reporta Path do erro")]
    public void EnderecoFuncionario_JsonMalFormado_ThrowJsonExceptionComPath()
    {
        var jsonInvalido = "{ \"cep\": \"01310-100\", \"logradouro\": "; // truncado

        var act = () => JsonSerializer.Deserialize<EnderecoFuncionario>(jsonInvalido, JsonOpts);

        var ex = act.Should().Throw<JsonException>().Which;
        ex.Message.Should().NotBeNullOrEmpty();
        ex.LineNumber.Should().NotBeNull();
        ex.BytePositionInLine.Should().NotBeNull();
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "EnderecoFuncionario")]
    [Fact(DisplayName = "Dado JSON com tipo incompatível (número em string), quando desserializa, então JsonException identifica a propriedade ofensora")]
    public void EnderecoFuncionario_JsonTipoErrado_ThrowJsonExceptionComPropriedade()
    {
        var jsonInvalido = "{ \"cep\": 12345, \"logradouro\": \"Rua A\" }"; // cep deveria ser string

        var act = () => JsonSerializer.Deserialize<EnderecoFuncionario>(jsonInvalido, JsonOpts);

        var ex = act.Should().Throw<JsonException>().Which;
        // System.Text.Json inclui o path da propriedade ofensora na mensagem (ex: "$.cep").
        ex.Path.Should().Be("$.cep");
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "ContaBancariaFuncionario")]
    [Fact(DisplayName = "Dado JSON vazio com aspas trocadas, quando desserializa ContaBancariaFuncionario, então JsonException é lançada com Path no início")]
    public void ContaBancariaFuncionario_JsonMalFormado_ThrowJsonException()
    {
        var jsonInvalido = "{ 'agencia': '0001' }"; // aspas simples não são JSON válido

        var act = () => JsonSerializer.Deserialize<ContaBancariaFuncionario>(jsonInvalido, JsonOpts);

        var ex = act.Should().Throw<JsonException>().Which;
        ex.LineNumber.Should().Be(0);
        ex.BytePositionInLine.Should().BeGreaterThanOrEqualTo(0);
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "ContaBancariaFuncionario")]
    [Fact(DisplayName = "Dado string vazia, quando desserializa ContaBancariaFuncionario, então JsonException com mensagem sobre falta de valor")]
    public void ContaBancariaFuncionario_StringVazia_ThrowJsonException()
    {
        var act = () => JsonSerializer.Deserialize<ContaBancariaFuncionario>("", JsonOpts);

        act.Should().Throw<JsonException>()
            .WithMessage("*input*"); // System.Text.Json reporta "input does not contain any JSON tokens" ou similar
    }

    // ---------------------------------------------- Enum.Parse com valor inválido

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "EnumsRhMensagensErro")]
    [Fact(DisplayName = "Dado string inexistente, quando Enum.Parse<TipoJornada>, então ArgumentException cita o valor recusado entre aspas")]
    public void TipoJornada_ParseInvalido_ThrowArgumentExceptionMencionandoValor()
    {
        var act = () => Enum.Parse<TipoJornada>("EscalaXTrocaY");

        var ex = act.Should().Throw<ArgumentException>().Which;
        ex.Message.Should().Contain("'EscalaXTrocaY'",
            ".NET 10+ retorna apenas \"Requested value 'X' was not found.\" — sem citar o tipo do enum");
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "EnumsRhMensagensErro")]
    [Fact(DisplayName = "Dado string em case errada com ignoreCase=false, quando Enum.Parse<TipoContrato>, então ArgumentException cita o valor 'clt' (minúsculo)")]
    public void TipoContrato_ParseCaseSensitive_RecusaMinusculo()
    {
        var act = () => Enum.Parse<TipoContrato>("clt", ignoreCase: false);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*clt*");
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "EnumsRhMensagensErro")]
    [Fact(DisplayName = "Dado string inexistente, quando Enum.TryParse<TipoBeneficio>, então retorna false e value=default sem lançar")]
    public void TipoBeneficio_TryParseInvalido_RetornaFalseSemExcecao()
    {
        var ok = Enum.TryParse<TipoBeneficio>("BeneficioFantasma", out var valor);

        ok.Should().BeFalse();
        valor.Should().Be(default(TipoBeneficio)); // default = ValeTransporte (0)
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "EnumsRhMensagensErro")]
    [Fact(DisplayName = "Dado inteiro fora do range definido, quando converte para TipoDependente, então cast cria valor undefined detectável por IsDefined")]
    public void TipoDependente_IntForaDoRange_DetectavelPorIsDefined()
    {
        var valorBogus = (TipoDependente)50;

        Enum.IsDefined(typeof(TipoDependente), valorBogus).Should().BeFalse(
            "código de importação que aceitar inteiros opacos deve validar com IsDefined antes de persistir");
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "EnumsRhMensagensErro")]
    [Fact(DisplayName = "Dado string nula, quando Enum.Parse<MotivoSalario>, então ArgumentNullException explicita o nome do parâmetro 'value'")]
    public void MotivoSalario_ParseNull_ThrowArgumentNullException()
    {
        var act = () => Enum.Parse<MotivoSalario>(null!);

        act.Should().Throw<ArgumentNullException>()
            .Which.ParamName.Should().Be("value");
    }
}
