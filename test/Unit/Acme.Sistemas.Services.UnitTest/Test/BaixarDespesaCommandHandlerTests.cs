using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Despesa.Command.BaixarDespesa;
using FluentAssertions;
using Moq;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class BaixarDespesaCommandHandlerTests
{
    private readonly Mock<IDespesaRepository> _despesas = new();
    private readonly Mock<ITenantContext> _tenant = new();

    public BaixarDespesaCommandHandlerTests()
    {
        _tenant.SetupGet(t => t.TenantId).Returns(Guid.NewGuid());
        _tenant.SetupGet(t => t.UserId).Returns(Guid.NewGuid());
    }

    [Fact]
    public async Task Baixar_DespesaPendente_AlteraStatusEPersistePagamento()
    {
        var id = Guid.NewGuid();
        _despesas.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Despesa
            {
                Id = id,
                Nome = "Conta de Luz",
                Valor = 200m,
                DataVencimento = DateTime.UtcNow.AddDays(2),
                StatusPagamento = StatusPagamento.Pendente
            });

        Despesa? capturada = null;
        _despesas.Setup(r => r.BaixarAsync(It.IsAny<Despesa>(), It.IsAny<CancellationToken>()))
            .Callback<Despesa, CancellationToken>((d, _) => capturada = d)
            .Returns(Task.CompletedTask);

        var sut = new BaixarDespesaCommandHandler(_despesas.Object, _tenant.Object);
        var hoje = DateTime.UtcNow.Date;
        var result = await sut.Handle(
            new BaixarDespesaCommand(id, 200m, hoje, FormaPagamento.Pix, "Pago via PIX"),
            default);

        result.IsSuccess.Should().BeTrue();
        capturada!.StatusPagamento.Should().Be(StatusPagamento.Pago);
        capturada.ValorPago.Should().Be(200m);
        capturada.DataPagamento.Should().Be(hoje);
        capturada.FormaPagamento.Should().Be(FormaPagamento.Pix);
    }

    [Fact]
    public async Task Baixar_DespesaJaPaga_RetornaConflict()
    {
        var id = Guid.NewGuid();
        _despesas.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Despesa { Id = id, StatusPagamento = StatusPagamento.Pago });

        var sut = new BaixarDespesaCommandHandler(_despesas.Object, _tenant.Object);
        var result = await sut.Handle(
            new BaixarDespesaCommand(id, 100m, DateTime.UtcNow, FormaPagamento.Pix, null), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(409);
    }

    [Fact]
    public async Task Baixar_DespesaInexistente_RetornaNotFound()
    {
        _despesas.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Despesa?)null);

        var sut = new BaixarDespesaCommandHandler(_despesas.Object, _tenant.Object);
        var result = await sut.Handle(
            new BaixarDespesaCommand(Guid.NewGuid(), 100m, DateTime.UtcNow, FormaPagamento.Pix, null), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(404);
    }
}
