using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Inventario.Command.FecharInventario;

public sealed class FecharInventarioCommandHandler
    : IRequestHandler<FecharInventarioCommand, ResponseDefault<FecharInventarioCommandResult>>
{
    private readonly IInventarioRepository _inventarios;
    private readonly IEstoqueProdutoRepository _saldos;
    private readonly IEntradaProdutoEstoqueRepository _entradas;
    private readonly ISaidaProdutoEstoqueRepository _saidas;
    private readonly ITenantContext _tenantContext;

    public FecharInventarioCommandHandler(
        IInventarioRepository inventarios,
        IEstoqueProdutoRepository saldos,
        IEntradaProdutoEstoqueRepository entradas,
        ISaidaProdutoEstoqueRepository saidas,
        ITenantContext tenantContext)
    {
        _inventarios = inventarios;
        _saldos = saldos;
        _entradas = entradas;
        _saidas = saidas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<FecharInventarioCommandResult>> Handle(FecharInventarioCommand request, CancellationToken cancellationToken)
    {
        var inventario = await _inventarios.GetByIdAsync(request.InventarioId, cancellationToken);
        if (inventario is null)
            return ResponseDefault<FecharInventarioCommandResult>.NotFound("Inventário não encontrado.");

        if (inventario.Status == StatusInventario.Fechado)
            return ResponseDefault<FecharInventarioCommandResult>.Conflict("Inventário já fechado.");
        if (inventario.Status == StatusInventario.Cancelado)
            return ResponseDefault<FecharInventarioCommandResult>.Conflict("Inventário cancelado.");

        var itens = (await _inventarios.ListItensAsync(inventario.Id, cancellationToken))
            .ToDictionary(i => i.ProdutoId, i => i);

        var ajustes = new List<AjusteGerado>();
        var now = DateTime.UtcNow;

        foreach (var contagem in request.Contagens)
        {
            if (!itens.TryGetValue(contagem.ProdutoId, out var item)) continue;

            item.SaldoContado = contagem.SaldoContado;
            item.Observacao = contagem.Observacao;
            await _inventarios.UpdateItemAsync(item, cancellationToken);

            var diferenca = contagem.SaldoContado - item.SaldoSistema;
            if (diferenca == 0) continue;

            if (diferenca > 0)
            {
                await _entradas.AddAsync(new EntradaProdutoEstoque
                {
                    TenantId = _tenantContext.TenantId,
                    EstoqueId = inventario.EstoqueId,
                    ProdutoId = contagem.ProdutoId,
                    Quantidade = diferenca,
                    QuantidadeRestante = diferenca,
                    Origem = OrigemMovimento.Inventario,
                    Motivo = $"Ajuste de inventário {inventario.Id}",
                    DocumentoReferencia = inventario.Id.ToString(),
                    DataMovimento = now,
                    CreatedBy = _tenantContext.UserId
                }, cancellationToken);
            }
            else
            {
                await _saidas.AddAsync(new SaidaProdutoEstoque
                {
                    TenantId = _tenantContext.TenantId,
                    EstoqueId = inventario.EstoqueId,
                    ProdutoId = contagem.ProdutoId,
                    Quantidade = Math.Abs(diferenca),
                    Origem = OrigemMovimento.Inventario,
                    Motivo = $"Ajuste de inventário {inventario.Id}",
                    DocumentoReferencia = inventario.Id.ToString(),
                    DataMovimento = now,
                    CreatedBy = _tenantContext.UserId
                }, cancellationToken);
            }

            await _saldos.AjustarSaldoAsync(inventario.EstoqueId, contagem.ProdutoId, diferenca, 0, cancellationToken);

            ajustes.Add(new AjusteGerado(contagem.ProdutoId, item.SaldoSistema, contagem.SaldoContado, diferenca));
        }

        await _inventarios.FecharAsync(inventario.Id, now, cancellationToken);

        return ResponseDefault<FecharInventarioCommandResult>.Ok(
            new FecharInventarioCommandResult(inventario.Id, ajustes.Count, ajustes));
    }
}
