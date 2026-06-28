using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Jornada.Command.AlterarJornada;
using Acme.Sistemas.Services.V1.Rh.Jornada.Command.CriarJornada;
using Acme.Sistemas.Services.V1.Rh.Jornada.Command.RemoverJornada;
using Acme.Sistemas.Services.V1.Rh.Jornada.Query.ListarJornadas;
using Acme.Sistemas.Services.V1.Rh.Jornada.Query.ObterJornada;
using FluentAssertions;
using Moq;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

/// <summary>
/// Cobertura dos 5 handlers do vertical Jornada (CRUD via CQRS): caminhos felizes
/// e principais erros (409 nome duplicado, 404 ID inexistente). Mocka
/// <see cref="IJornadaRepository"/> e <see cref="ITenantContext"/>.
/// </summary>
public class JornadaHandlersTests
{
    private readonly Mock<IJornadaRepository> _repo = new();
    private readonly Mock<ITenantContext> _tenant = new();
    private readonly Guid _tenantId = Guid.NewGuid();
    private readonly Guid _userId = Guid.NewGuid();

    public JornadaHandlersTests()
    {
        _tenant.SetupGet(t => t.TenantId).Returns(_tenantId);
        _tenant.SetupGet(t => t.UserId).Returns(_userId);
    }

    // -------------------------------------------------------------- CriarJornada

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CriarJornada")]
    [Fact(DisplayName = "Dado nome inédito, quando CriarJornada, então persiste e retorna 201 com Id+Nome")]
    public async Task CriarJornada_NomeInedito_CriaERetorna201()
    {
        _repo.Setup(r => r.GetByNomeAsync("44h CLT", It.IsAny<CancellationToken>()))
             .ReturnsAsync((Jornada?)null);

        Jornada? capturada = null;
        _repo.Setup(r => r.AddAsync(It.IsAny<Jornada>(), It.IsAny<CancellationToken>()))
             .Callback<Jornada, CancellationToken>((j, _) => capturada = j)
             .Returns(Task.CompletedTask);

        var sut = new CriarJornadaCommandHandler(_repo.Object, _tenant.Object);
        var result = await sut.Handle(new CriarJornadaCommand(
            "44h CLT", TipoJornada.Fixa, 44m, 8m, "[]"), default);

        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(201);
        result.Content!.Nome.Should().Be("44h CLT");
        capturada.Should().NotBeNull();
        capturada!.TenantId.Should().Be(_tenantId);
        capturada.CreatedBy.Should().Be(_userId);
        capturada.Ativo.Should().BeTrue();
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CriarJornada")]
    [Fact(DisplayName = "Dado nome já em uso no tenant, quando CriarJornada, então retorna 409 sem persistir")]
    public async Task CriarJornada_NomeDuplicado_RetornaConflictSemPersistir()
    {
        _repo.Setup(r => r.GetByNomeAsync("44h CLT", It.IsAny<CancellationToken>()))
             .ReturnsAsync(new Jornada { Nome = "44h CLT" });

        var sut = new CriarJornadaCommandHandler(_repo.Object, _tenant.Object);
        var result = await sut.Handle(new CriarJornadaCommand(
            "44h CLT", TipoJornada.Fixa, 44m, 8m, "[]"), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(409);
        result.Message.Should().Contain("44h CLT");
        _repo.Verify(r => r.AddAsync(It.IsAny<Jornada>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // -------------------------------------------------------------- AlterarJornada

    [Trait("Solucao", "Services")]
    [Trait("Acao", "AlterarJornada")]
    [Fact(DisplayName = "Dado ID inexistente, quando AlterarJornada, então retorna 404")]
    public async Task AlterarJornada_IdInexistente_RetornaNotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((Jornada?)null);

        var sut = new AlterarJornadaCommandHandler(_repo.Object, _tenant.Object);
        var result = await sut.Handle(new AlterarJornadaCommand(
            Guid.NewGuid(), "X", TipoJornada.Fixa, 44m, 8m, "[]", true, 10, true), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(404);
        _repo.Verify(r => r.UpdateAsync(It.IsAny<Jornada>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "AlterarJornada")]
    [Fact(DisplayName = "Dado renomeio para nome já em uso por outra jornada, quando AlterarJornada, então retorna 409")]
    public async Task AlterarJornada_RenomeioColidente_RetornaConflict()
    {
        var idAtual = Guid.NewGuid();
        var idConflito = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(idAtual, It.IsAny<CancellationToken>()))
             .ReturnsAsync(new Jornada { Id = idAtual, Nome = "Original", JanelasJson = "[]" });
        _repo.Setup(r => r.GetByNomeAsync("Nome Em Uso", It.IsAny<CancellationToken>()))
             .ReturnsAsync(new Jornada { Id = idConflito, Nome = "Nome Em Uso" });

        var sut = new AlterarJornadaCommandHandler(_repo.Object, _tenant.Object);
        var result = await sut.Handle(new AlterarJornadaCommand(
            idAtual, "Nome Em Uso", TipoJornada.Fixa, 44m, 8m, "[]", true, 10, true), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(409);
        _repo.Verify(r => r.UpdateAsync(It.IsAny<Jornada>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "AlterarJornada")]
    [Fact(DisplayName = "Dado jornada existente, quando AlterarJornada com novos valores, então UpdateAsync recebe a entidade com campos atualizados e UpdatedBy do contexto")]
    public async Task AlterarJornada_OK_PersisteCamposEUpdatedBy()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
             .ReturnsAsync(new Jornada { Id = id, Nome = "Antiga", JanelasJson = "[]", ToleranciaMinutos = 5 });

        Jornada? capturada = null;
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Jornada>(), It.IsAny<CancellationToken>()))
             .Callback<Jornada, CancellationToken>((j, _) => capturada = j)
             .Returns(Task.CompletedTask);

        var sut = new AlterarJornadaCommandHandler(_repo.Object, _tenant.Object);
        var result = await sut.Handle(new AlterarJornadaCommand(
            id, "Antiga", TipoJornada.Escala12x36, 42m, 12m, "[{}]", false, 20, false), default);

        result.IsSuccess.Should().BeTrue();
        capturada.Should().NotBeNull();
        capturada!.Tipo.Should().Be(TipoJornada.Escala12x36);
        capturada.CargaSemanalHoras.Should().Be(42m);
        capturada.ToleranciaMinutos.Should().Be(20);
        capturada.PermiteMarcarIntervalo.Should().BeFalse();
        capturada.Ativo.Should().BeFalse();
        capturada.UpdatedBy.Should().Be(_userId);
    }

    // -------------------------------------------------------------- RemoverJornada

    [Trait("Solucao", "Services")]
    [Trait("Acao", "RemoverJornada")]
    [Fact(DisplayName = "Dado ID inexistente, quando RemoverJornada, então retorna 404 sem chamar DeleteAsync")]
    public async Task RemoverJornada_IdInexistente_RetornaNotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((Jornada?)null);

        var sut = new RemoverJornadaCommandHandler(_repo.Object);
        var result = await sut.Handle(new RemoverJornadaCommand(Guid.NewGuid()), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(404);
        _repo.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "RemoverJornada")]
    [Fact(DisplayName = "Dado jornada existente, quando RemoverJornada, então chama DeleteAsync (soft) e retorna 200")]
    public async Task RemoverJornada_OK_ChamaDeleteAsync()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
             .ReturnsAsync(new Jornada { Id = id });

        var sut = new RemoverJornadaCommandHandler(_repo.Object);
        var result = await sut.Handle(new RemoverJornadaCommand(id), default);

        result.IsSuccess.Should().BeTrue();
        result.Content!.Id.Should().Be(id);
        _repo.Verify(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    // -------------------------------------------------------------- ObterJornada

    [Trait("Solucao", "Services")]
    [Trait("Acao", "ObterJornada")]
    [Fact(DisplayName = "Dado ID inexistente, quando ObterJornada, então retorna 404")]
    public async Task ObterJornada_IdInexistente_RetornaNotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((Jornada?)null);

        var sut = new ObterJornadaQueryHandler(_repo.Object);
        var result = await sut.Handle(new ObterJornadaQuery(Guid.NewGuid()), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(404);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "ObterJornada")]
    [Fact(DisplayName = "Dado jornada existente, quando ObterJornada, então retorna todos os campos incluindo janelas_json")]
    public async Task ObterJornada_OK_RetornaCamposCompletos()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
             .ReturnsAsync(new Jornada
             {
                 Id = id,
                 Nome = "12x36",
                 Tipo = TipoJornada.Escala12x36,
                 CargaSemanalHoras = 42m,
                 CargaDiariaHoras = 12m,
                 JanelasJson = "[{\"dia\":\"seg\"}]",
                 PermiteMarcarIntervalo = false,
                 ToleranciaMinutos = 15,
                 Ativo = true,
             });

        var sut = new ObterJornadaQueryHandler(_repo.Object);
        var result = await sut.Handle(new ObterJornadaQuery(id), default);

        result.IsSuccess.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content!.Nome.Should().Be("12x36");
        result.Content.JanelasJson.Should().Contain("seg");
        result.Content.PermiteMarcarIntervalo.Should().BeFalse();
        result.Content.ToleranciaMinutos.Should().Be(15);
    }

    // -------------------------------------------------------------- ListarJornadas

    [Trait("Solucao", "Services")]
    [Trait("Acao", "ListarJornadas")]
    [Fact(DisplayName = "Dado repositório com 3 jornadas, quando ListarJornadas, então retorna 3 itens + total=3")]
    public async Task ListarJornadas_RepositorioCom3_RetornaItensComTotal()
    {
        _repo.Setup(r => r.ListAsync(0, 50, It.IsAny<CancellationToken>()))
             .ReturnsAsync(new[]
             {
                 new Jornada { Nome = "A", JanelasJson = "[]" },
                 new Jornada { Nome = "B", JanelasJson = "[]" },
                 new Jornada { Nome = "C", JanelasJson = "[]" },
             });
        _repo.Setup(r => r.CountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(3);

        var sut = new ListarJornadasQueryHandler(_repo.Object);
        var result = await sut.Handle(new ListarJornadasQuery(), default);

        result.IsSuccess.Should().BeTrue();
        result.Content!.Items.Should().HaveCount(3);
        result.Content.Total.Should().Be(3);
        result.Content.Items.Select(i => i.Nome).Should().BeEquivalentTo(new[] { "A", "B", "C" });
    }

    // -------------------------------------------------------------- Validation

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CriarJornada")]
    [Theory(DisplayName = "Dado payload de CriarJornada inválido, quando valida, então gera erro com mensagem específica")]
    [InlineData("", TipoJornada.Fixa, 44, "[]", "Nome")]
    [InlineData("OK", TipoJornada.Fixa, 0, "[]", "CargaSemanalHoras")]
    [InlineData("OK", TipoJornada.Fixa, 61, "[]", "CargaSemanalHoras")]
    [InlineData("OK", TipoJornada.Fixa, 44, "{nao-eh-json", "JanelasJson")]
    [InlineData("OK", TipoJornada.Fixa, 44, "", "JanelasJson")]
    public void CriarJornadaValidation_PayloadInvalido_ProduzErro(
        string nome, TipoJornada tipo, decimal cargaSemanal, string janelas, string campoEsperado)
    {
        var validator = new CriarJornadaCommandValidation();
        var result = validator.Validate(new CriarJornadaCommand(
            nome, tipo, cargaSemanal, 8m, janelas));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == campoEsperado,
            $"deve apontar erro no campo `{campoEsperado}`");
    }
}
