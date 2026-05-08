using Acme.Sistemas.Core.Helper;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.ExternalIntegration.Clients.ViaCep;
using Acme.Sistemas.Services.V1.Cliente.Command.CriarCliente;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;
using FluentAssertions;
using Moq;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class CriarClienteCommandHandlerTests
{
    private readonly Mock<IClienteRepository> _repo = new();
    private readonly Mock<IViaCepExternalClient> _viaCep = new();
    private readonly Mock<ITenantContext> _tenant = new();

    public CriarClienteCommandHandlerTests()
    {
        _tenant.SetupGet(t => t.TenantId).Returns(Guid.NewGuid());
        _tenant.SetupGet(t => t.UserId).Returns(Guid.NewGuid());
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CriarCliente")]
    [Fact(DisplayName = "Dado documento já existente, quando criar cliente, então retorna 409 Conflict e não persiste")]
    public async Task Criar_DocumentoExistente_RetornaConflict()
    {
        _repo.Setup(r => r.GetByDocumentoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Cliente { Documento = "12345678901" });

        var sut = new CriarClienteCommandHandler(_repo.Object, _viaCep.Object, _tenant.Object);
        var result = await sut.Handle(
            new CriarClienteCommand(TipoPessoa.Fisica, "João", null, "123.456.789-01",
                null, null, null, null), default);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(409);
        _repo.Verify(r => r.AddAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CriarCliente")]
    [Fact(DisplayName = "Dado documento inédito, quando criar cliente, então persiste com TenantId e CreatedBy do contexto")]
    public async Task Criar_NovoCliente_PersisteComTenantContext()
    {
        _repo.Setup(r => r.GetByDocumentoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        Cliente? capturado = null;
        _repo.Setup(r => r.AddAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .Callback<Cliente, CancellationToken>((c, _) => capturado = c)
            .Returns(Task.CompletedTask);

        var sut = new CriarClienteCommandHandler(_repo.Object, _viaCep.Object, _tenant.Object);
        var result = await sut.Handle(
            new CriarClienteCommand(TipoPessoa.Fisica, "Maria", null, "987.654.321-00",
                null, "maria@example.com", "11999999999", null), default);

        result.IsSuccess.Should().BeTrue();
        capturado!.TenantId.Should().Be(_tenant.Object.TenantId);
        capturado.Documento.Should().Be("98765432100");
        capturado.CreatedBy.Should().Be(_tenant.Object.UserId);
    }
}
