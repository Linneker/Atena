using System.Text.Json;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Entities.Referencia;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

/// <summary>
/// Cobertura unitária para as entidades introduzidas pela Fase 1 do rh-fundacao (W1):
/// defaults imutáveis, estabilidade dos valores inteiros de enums (qualquer reordenação
/// rompe a migração de dados persistidos), e round-trip JSON dos value objects
/// EnderecoFuncionario / ContaBancariaFuncionario que são serializados nas colunas JSON
/// de funcionarios.
/// </summary>
public class EntidadesRhFundacaoTests
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    // ------------------------------------------------------------------- Defaults

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "Jornada")]
    [Fact(DisplayName = "Dado nova Jornada, quando construída, então tem defaults seguros (ativa, tolerância 10min, intervalo permitido, janelas vazias)")]
    public void Jornada_NovaInstancia_DefaultsSeguros()
    {
        var j = new Jornada();

        j.Ativo.Should().BeTrue();
        j.ToleranciaMinutos.Should().Be(10);
        j.PermiteMarcarIntervalo.Should().BeTrue();
        j.JanelasJson.Should().Be("[]");
        j.Id.Should().NotBeEmpty();
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "Cargo")]
    [Fact(DisplayName = "Dado novo Cargo, quando construído, então é Ativo=true e Codigo/Cbo nulos")]
    public void Cargo_NovaInstancia_AtivoPorPadraoCboNulo()
    {
        var c = new Cargo();

        c.Ativo.Should().BeTrue();
        c.Codigo.Should().BeNull();
        c.CodigoCbo.Should().BeNull();
        c.SalarioBaseSugerido.Should().BeNull();
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "Departamento")]
    [Fact(DisplayName = "Dado novo Departamento, quando construído, então é Ativo=true e CentroDeCusto nulo")]
    public void Departamento_NovaInstancia_AtivoPorPadrao()
    {
        var d = new Departamento();

        d.Ativo.Should().BeTrue();
        d.CentroDeCustoId.Should().BeNull();
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "Lotacao")]
    [Fact(DisplayName = "Dado nova Lotacao, quando construída, então é Ativo=true e Empresa/Cnpj nulos")]
    public void Lotacao_NovaInstancia_AtivoPorPadrao()
    {
        var l = new Lotacao();

        l.Ativo.Should().BeTrue();
        l.EmpresaId.Should().BeNull();
        l.Cnpj.Should().BeNull();
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "BeneficioCatalogo")]
    [Fact(DisplayName = "Dado novo BeneficioCatalogo, quando construído, então é Ativo=true e Tipo=ValeTransporte (default 0)")]
    public void BeneficioCatalogo_NovaInstancia_DefaultsSeguros()
    {
        var b = new BeneficioCatalogo();

        b.Ativo.Should().BeTrue();
        b.Tipo.Should().Be(TipoBeneficio.ValeTransporte);
        b.DescontoFuncionarioPct.Should().BeNull();
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "Dependente")]
    [Fact(DisplayName = "Dado novo Dependente, quando construído, então Irrf=false e SalarioFamilia=false (não conta sem ato explícito)")]
    public void Dependente_NovaInstancia_FlagsLegaisFalsasPorPadrao()
    {
        var d = new Dependente();

        d.Irrf.Should().BeFalse();
        d.SalarioFamilia.Should().BeFalse();
        d.PensaoAlimenticiaPct.Should().BeNull();
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "Funcionario")]
    [Fact(DisplayName = "Dado novo Funcionario, quando construído, então Status=Ativo, Nacionalidade=Brasileira e campos RH iniciam nulos")]
    public void Funcionario_NovaInstancia_DefaultsRh()
    {
        var f = new Funcionario();

        f.Status.Should().Be(StatusAtivo.Ativo);
        f.Nacionalidade.Should().Be("Brasileira");
        f.CargoId.Should().BeNull();
        f.LotacaoId.Should().BeNull();
        f.DepartamentoId.Should().BeNull();
        f.TipoContrato.Should().BeNull();
        f.RegimeRemuneracao.Should().BeNull();
        f.Endereco.Should().BeNull();
        f.ContaBancaria.Should().BeNull();
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "Cbo")]
    [Fact(DisplayName = "Dado novo Cbo, quando construído, então é Ativo=true e usa código natural (catálogo nacional sem TenantId)")]
    public void Cbo_NovaInstancia_NaoExtendeBaseEntity()
    {
        var c = new Cbo();

        c.Ativo.Should().BeTrue();
        // Cbo é catálogo nacional, não deve carregar tenant_id nem audit fields
        typeof(Cbo).BaseType.Should().Be(typeof(object));
    }

    // ------------------------------------------------------------- Estabilidade de enums

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "EnumsRhEstaveis")]
    [Theory(DisplayName = "Dado os enums RH, quando consulta valor inteiro, então mantém ordem estável (qualquer mudança quebra dados persistidos)")]
    [InlineData(typeof(TipoJornada), "Fixa", 0)]
    [InlineData(typeof(TipoJornada), "Escala12x36", 1)]
    [InlineData(typeof(TipoJornada), "Estagio", 5)]
    [InlineData(typeof(MotivoSalario), "Admissao", 0)]
    [InlineData(typeof(MotivoSalario), "Outro", 99)]
    [InlineData(typeof(TipoBeneficio), "ValeTransporte", 0)]
    [InlineData(typeof(TipoBeneficio), "PlanoSaude", 3)]
    [InlineData(typeof(TipoBeneficio), "Outro", 99)]
    [InlineData(typeof(TipoDependente), "Filho", 0)]
    [InlineData(typeof(TipoDependente), "Conjuge", 2)]
    [InlineData(typeof(TipoContrato), "Clt", 0)]
    [InlineData(typeof(TipoContrato), "EstagioRemunerado", 1)]
    [InlineData(typeof(RegimeRemuneracao), "Mensalista", 0)]
    [InlineData(typeof(RegimeRemuneracao), "Horista", 1)]
    [InlineData(typeof(EstadoCivil), "Solteiro", 0)]
    public void Enums_Valores_Estaveis(Type enumType, string nome, int valorEsperado)
    {
        var value = (int)Enum.Parse(enumType, nome);
        value.Should().Be(valorEsperado);
    }

    // -------------------------------------------------------- Round-trip JSON dos value objects

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "EnderecoFuncionario")]
    [Fact(DisplayName = "Dado EnderecoFuncionario preenchido, quando serializa e desserializa via JSON camelCase, então preserva todos os campos")]
    public void EnderecoFuncionario_JsonRoundTrip_PreservaTodosCampos()
    {
        var original = new EnderecoFuncionario
        {
            Cep = "01310-100",
            Logradouro = "Av. Paulista",
            Numero = "1578",
            Complemento = "Sala 12",
            Bairro = "Bela Vista",
            Cidade = "São Paulo",
            Uf = "SP",
            Pais = "BR"
        };

        var json = JsonSerializer.Serialize(original, JsonOpts);
        var rehydrated = JsonSerializer.Deserialize<EnderecoFuncionario>(json, JsonOpts);

        rehydrated.Should().BeEquivalentTo(original);
        json.Should().Contain("\"cep\":").And.Contain("\"logradouro\":");
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "EnderecoFuncionario")]
    [Fact(DisplayName = "Dado EnderecoFuncionario novo, quando construído, então Pais default é BR")]
    public void EnderecoFuncionario_NovaInstancia_PaisDefaultBR()
    {
        var e = new EnderecoFuncionario();

        e.Pais.Should().Be("BR");
        e.Cep.Should().BeNull();
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "ContaBancariaFuncionario")]
    [Fact(DisplayName = "Dado ContaBancariaFuncionario preenchida, quando serializa e desserializa via JSON, então preserva banco, agência, conta, dígito e Pix")]
    public void ContaBancariaFuncionario_JsonRoundTrip_PreservaTodosCampos()
    {
        var original = new ContaBancariaFuncionario
        {
            CodigoBanco = "260",
            NomeBanco = "Nubank",
            Agencia = "0001",
            AgenciaDigito = null,
            Conta = "12345678",
            ContaDigito = "9",
            TipoConta = "Corrente",
            ChavePix = "linneker@example.com"
        };

        var json = JsonSerializer.Serialize(original, JsonOpts);
        var rehydrated = JsonSerializer.Deserialize<ContaBancariaFuncionario>(json, JsonOpts);

        rehydrated.Should().BeEquivalentTo(original);
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "Funcionario")]
    [Fact(DisplayName = "Dado Funcionario com Endereco e ContaBancaria atribuídos, quando relê propriedades, então retornam as instâncias atribuídas")]
    public void Funcionario_PropriedadesNested_Atribuiveis()
    {
        var f = new Funcionario
        {
            Endereco = new EnderecoFuncionario { Cep = "01310-100", Uf = "SP" },
            ContaBancaria = new ContaBancariaFuncionario { CodigoBanco = "001" }
        };

        f.Endereco!.Cep.Should().Be("01310-100");
        f.ContaBancaria!.CodigoBanco.Should().Be("001");
    }
}
