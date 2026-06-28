using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CriarFuncionarioCompleto;
using Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RegistrarReajusteSalarial;
using FluentAssertions;
using Moq;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

/// <summary>
/// Cobertura dos handlers críticos do Funcionário RH: <c>CriarFuncionarioCompleto</c>
/// (CLT/Estágio/Aprendiz com dependentes+benefícios+escala) e <c>RegistrarReajusteSalarial</c>
/// (fecha vigência anterior + cria nova linha).
/// </summary>
public class FuncionarioCompletoHandlersTests
{
    private readonly Mock<IFuncionarioRepository> _funcRepo = new();
    private readonly Mock<IHistoricoSalarioRepository> _histRepo = new();
    private readonly Mock<IEscalaFuncionarioRepository> _escalaRepo = new();
    private readonly Mock<IBeneficioFuncionarioRepository> _benefRepo = new();
    private readonly Mock<IDependenteRepository> _depRepo = new();
    private readonly Mock<ITenantContext> _tenant = new();
    private readonly Guid _tenantId = Guid.NewGuid();
    private readonly Guid _userId = Guid.NewGuid();

    public FuncionarioCompletoHandlersTests()
    {
        _tenant.SetupGet(t => t.TenantId).Returns(_tenantId);
        _tenant.SetupGet(t => t.UserId).Returns(_userId);
    }

    // ============================== CriarFuncionarioCompleto

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CriarFuncionarioCompleto")]
    [Fact(DisplayName = "Dado CLT com 2 dependentes + 1 benefício + escala, quando CriarFuncionarioCompleto, então cria tudo atomicamente e retorna 201 com contagens")]
    public async Task CriarFuncionarioCompleto_CltComExtras_CriaTudo()
    {
        _funcRepo.Setup(r => r.GetByCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Funcionario?)null);
        _funcRepo.Setup(r => r.GetByMatriculaAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Funcionario?)null);

        var sut = NewSut();
        var cmd = new CriarFuncionarioCompletoCommand(
            NomeCompleto: "Maria Souza",
            Cpf: "39053344705",
            Email: null, Telefone: null, DataNascimento: null,
            EstadoCivil: null, Naturalidade: null, Nacionalidade: null,
            Rg: null, RgOrgao: null, RgUf: null, Endereco: null,
            DataAdmissao: new DateOnly(2026, 1, 15),
            CargoId: Guid.NewGuid(), LotacaoId: null, DepartamentoId: null, CentroDeCustoId: null,
            TipoContrato: TipoContrato.Clt, RegimeRemuneracao: RegimeRemuneracao.Mensalista,
            CodigoMatricula: "0001", Pis: "12056412829", Ctps: null, CtpsSerie: null, CtpsUf: null,
            SalarioInicial: 3500m, ContaBancaria: null,
            JornadaId: Guid.NewGuid(),
            Beneficios: new[]
            {
                new BeneficioInicialDto(Guid.NewGuid(), 250m, null, new DateOnly(2026, 1, 15)),
            },
            Dependentes: new[]
            {
                new DependenteInicialDto("Filho 1", "12345678909", new DateOnly(2020, 5, 1), TipoDependente.Filho, Irrf: true),
                new DependenteInicialDto("Filho 2", null, new DateOnly(2022, 8, 1), TipoDependente.Filho),
            });

        var result = await sut.Handle(cmd, default);

        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(201);
        result.Content!.BeneficiosCriados.Should().Be(1);
        result.Content.DependentesCriados.Should().Be(2);
        result.Content.EscalaId.Should().NotBeNull();

        _funcRepo.Verify(r => r.AddAsync(It.Is<Funcionario>(f =>
            f.TenantId == _tenantId && f.CreatedBy == _userId && f.Cpf == "39053344705"),
            It.IsAny<CancellationToken>()), Times.Once);
        _histRepo.Verify(r => r.AddAsync(It.Is<HistoricoSalario>(h =>
            h.Valor == 3500m && h.Motivo == MotivoSalario.Admissao),
            It.IsAny<CancellationToken>()), Times.Once);
        _escalaRepo.Verify(r => r.AddAsync(It.IsAny<EscalaFuncionario>(),
            It.IsAny<CancellationToken>()), Times.Once);
        _benefRepo.Verify(r => r.AddAsync(It.IsAny<BeneficioFuncionario>(),
            It.IsAny<CancellationToken>()), Times.Once);
        _depRepo.Verify(r => r.AddAsync(It.IsAny<Dependente>(),
            It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CriarFuncionarioCompleto")]
    [Fact(DisplayName = "Dado CPF já existe, quando CriarFuncionarioCompleto, então retorna 409 Conflict sem inserir")]
    public async Task CriarFuncionarioCompleto_CpfDuplicado_RetornaConflict()
    {
        _funcRepo.Setup(r => r.GetByCpfAsync("39053344705", It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new Funcionario { Id = Guid.NewGuid(), Cpf = "39053344705" });

        var sut = NewSut();
        var result = await sut.Handle(BasicCmd("39053344705"), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(409);
        _funcRepo.Verify(r => r.AddAsync(It.IsAny<Funcionario>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CriarFuncionarioCompleto")]
    [Fact(DisplayName = "Dado estágio sem benefícios nem dependentes, quando CriarFuncionarioCompleto, então cria só funcionário + salário inicial")]
    public async Task CriarFuncionarioCompleto_EstagioMinimo_CriaApenasFuncEHistorico()
    {
        _funcRepo.Setup(r => r.GetByCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Funcionario?)null);

        var sut = NewSut();
        var cmd = new CriarFuncionarioCompletoCommand(
            NomeCompleto: "Estagiário", Cpf: "11144477735",
            Email: null, Telefone: null, DataNascimento: null,
            EstadoCivil: null, Naturalidade: null, Nacionalidade: null,
            Rg: null, RgOrgao: null, RgUf: null, Endereco: null,
            DataAdmissao: new DateOnly(2026, 2, 1),
            CargoId: null, LotacaoId: null, DepartamentoId: null, CentroDeCustoId: null,
            TipoContrato: TipoContrato.EstagioRemunerado, RegimeRemuneracao: RegimeRemuneracao.Mensalista,
            CodigoMatricula: null, Pis: null, Ctps: null, CtpsSerie: null, CtpsUf: null,
            SalarioInicial: 1200m, ContaBancaria: null, JornadaId: null);

        var result = await sut.Handle(cmd, default);

        result.IsSuccess.Should().BeTrue();
        result.Content!.BeneficiosCriados.Should().Be(0);
        result.Content.DependentesCriados.Should().Be(0);
        result.Content.EscalaId.Should().BeNull();
        _histRepo.Verify(r => r.AddAsync(It.IsAny<HistoricoSalario>(), It.IsAny<CancellationToken>()), Times.Once);
        _escalaRepo.Verify(r => r.AddAsync(It.IsAny<EscalaFuncionario>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // ============================== RegistrarReajusteSalarial

    [Trait("Solucao", "Services")]
    [Trait("Acao", "RegistrarReajusteSalarial")]
    [Fact(DisplayName = "Dado reajuste em vigência existente sem fim, quando RegistrarReajuste, então fecha vigência anterior em D-1 e cria nova linha")]
    public async Task RegistrarReajuste_FechaVigenciaAnterior_ECriaNova()
    {
        var funcId = Guid.NewGuid();
        var vigenteAnteriorId = Guid.NewGuid();

        _funcRepo.Setup(r => r.GetByIdAsync(funcId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new Funcionario { Id = funcId });

        _histRepo.Setup(r => r.GetVigenteAsync(funcId, It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new HistoricoSalario
                 {
                     Id = vigenteAnteriorId, FuncionarioId = funcId,
                     Valor = 3000m, VigenciaInicio = new DateOnly(2025, 1, 1), VigenciaFim = null,
                 });

        var sut = new RegistrarReajusteSalarialCommandHandler(_funcRepo.Object, _histRepo.Object, _tenant.Object);
        var result = await sut.Handle(new RegistrarReajusteSalarialCommand(
            funcId, NovoValor: 3500m,
            VigenciaInicio: new DateOnly(2026, 1, 1),
            Motivo: MotivoSalario.Reajuste, Observacao: "5% anual"), default);

        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(201);
        result.Content!.VigenciaAnteriorFechadaId.Should().Be(vigenteAnteriorId);
        _histRepo.Verify(r => r.FecharVigenciaAsync(
            vigenteAnteriorId,
            new DateOnly(2025, 12, 31),
            _userId,
            It.IsAny<CancellationToken>()), Times.Once);
        _histRepo.Verify(r => r.AddAsync(
            It.Is<HistoricoSalario>(h => h.Valor == 3500m && h.Motivo == MotivoSalario.Reajuste),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "RegistrarReajusteSalarial")]
    [Fact(DisplayName = "Dado funcionário inexistente, quando RegistrarReajuste, então retorna 404 sem mexer no histórico")]
    public async Task RegistrarReajuste_FuncionarioInexistente_Retorna404()
    {
        var funcId = Guid.NewGuid();
        _funcRepo.Setup(r => r.GetByIdAsync(funcId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Funcionario?)null);

        var sut = new RegistrarReajusteSalarialCommandHandler(_funcRepo.Object, _histRepo.Object, _tenant.Object);
        var result = await sut.Handle(new RegistrarReajusteSalarialCommand(
            funcId, 3500m, new DateOnly(2026, 1, 1), MotivoSalario.Reajuste, null), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(404);
        _histRepo.Verify(r => r.AddAsync(It.IsAny<HistoricoSalario>(), It.IsAny<CancellationToken>()), Times.Never);
        _histRepo.Verify(r => r.FecharVigenciaAsync(
            It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "RegistrarReajusteSalarial")]
    [Fact(DisplayName = "Dado nova vigência anterior ao início da vigência atual, quando RegistrarReajuste, então retorna 409 (anti-overlap)")]
    public async Task RegistrarReajuste_VigenciaInvertida_RetornaConflict()
    {
        var funcId = Guid.NewGuid();
        _funcRepo.Setup(r => r.GetByIdAsync(funcId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new Funcionario { Id = funcId });
        _histRepo.Setup(r => r.GetVigenteAsync(funcId, It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new HistoricoSalario
                 {
                     Id = Guid.NewGuid(), FuncionarioId = funcId, Valor = 3000m,
                     VigenciaInicio = new DateOnly(2026, 6, 1), VigenciaFim = null,
                 });

        var sut = new RegistrarReajusteSalarialCommandHandler(_funcRepo.Object, _histRepo.Object, _tenant.Object);
        var result = await sut.Handle(new RegistrarReajusteSalarialCommand(
            funcId, 3500m, new DateOnly(2026, 1, 1), MotivoSalario.Reajuste, null), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(409);
    }

    // ============================== Helpers

    private CriarFuncionarioCompletoCommandHandler NewSut() => new(
        _funcRepo.Object, _histRepo.Object, _escalaRepo.Object,
        _benefRepo.Object, _depRepo.Object, _tenant.Object);

    private static CriarFuncionarioCompletoCommand BasicCmd(string cpf) => new(
        NomeCompleto: "X", Cpf: cpf,
        Email: null, Telefone: null, DataNascimento: null,
        EstadoCivil: null, Naturalidade: null, Nacionalidade: null,
        Rg: null, RgOrgao: null, RgUf: null, Endereco: null,
        DataAdmissao: new DateOnly(2026, 1, 1),
        CargoId: null, LotacaoId: null, DepartamentoId: null, CentroDeCustoId: null,
        TipoContrato: TipoContrato.Clt, RegimeRemuneracao: RegimeRemuneracao.Mensalista,
        CodigoMatricula: null, Pis: null, Ctps: null, CtpsSerie: null, CtpsUf: null,
        SalarioInicial: 1500m, ContaBancaria: null, JornadaId: null);
}
