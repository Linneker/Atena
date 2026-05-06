using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Interfaces.Repository;
using InventarioEntity = Acme.Sistemas.Domain.Entities.Estoque.Inventario;

namespace Acme.Sistemas.Services.V1.Inventario.Command.AbrirInventario;

public sealed class AbrirInventarioCommandHandler
    : IRequestHandler<AbrirInventarioCommand, ResponseDefault<AbrirInventarioCommandResult>>
{
    private readonly IInventarioRepository _inventarios;
    private readonly IEstoqueProdutoRepository _saldos;
    private readonly IEstoqueRepository _estoques;
    private readonly ITenantContext _tenantContext;

    public AbrirInventarioCommandHandler(
        IInventarioRepository inventarios,
        IEstoqueProdutoRepository saldos,
        IEstoqueRepository estoques,
        ITenantContext tenantContext)
    {
        _inventarios = inventarios;
        _saldos = saldos;
        _estoques = estoques;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AbrirInventarioCommandResult>> Handle(AbrirInventarioCommand request, CancellationToken cancellationToken)
    {
        var estoque = await _estoques.GetByIdAsync(request.EstoqueId, cancellationToken);
        if (estoque is null)
            return ResponseDefault<AbrirInventarioCommandResult>.NotFound("Estoque não encontrado.");

        // Snapshot dos saldos atuais
        var saldosAtuais = await _saldos.ListByEstoqueAsync(request.EstoqueId, 0, 100_000, cancellationToken);

        var inventario = new InventarioEntity
        {
            TenantId = _tenantContext.TenantId,
            EstoqueId = request.EstoqueId,
            DataAbertura = DateTime.UtcNow,
            Status = StatusInventario.EmContagem,
            Observacao = request.Observacao,
            CreatedBy = _tenantContext.UserId
        };

        await _inventarios.AddAsync(inventario, cancellationToken);

        var itens = saldosAtuais.Select(s => new InventarioItem
        {
            TenantId = _tenantContext.TenantId,
            InventarioId = inventario.Id,
            ProdutoId = s.ProdutoId,
            SaldoSistema = s.SaldoTotal,
            CreatedBy = _tenantContext.UserId
        }).ToList();

        await _inventarios.AddItensAsync(itens, cancellationToken);

        return ResponseDefault<AbrirInventarioCommandResult>.Created(
            new AbrirInventarioCommandResult(
                inventario.Id, inventario.EstoqueId, itens.Count, inventario.DataAbertura));
    }
}
