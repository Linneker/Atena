using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Estoque.Command.RegistrarEntrada;

public sealed class RegistrarEntradaCommandHandler
    : IRequestHandler<RegistrarEntradaCommand, ResponseDefault<RegistrarEntradaCommandResult>>
{
    private readonly IEstoqueRepository _estoques;
    private readonly IEstoqueProdutoRepository _saldos;
    private readonly IEntradaProdutoEstoqueRepository _entradas;
    private readonly ITenantContext _tenantContext;

    public RegistrarEntradaCommandHandler(
        IEstoqueRepository estoques,
        IEstoqueProdutoRepository saldos,
        IEntradaProdutoEstoqueRepository entradas,
        ITenantContext tenantContext)
    {
        _estoques = estoques;
        _saldos = saldos;
        _entradas = entradas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<RegistrarEntradaCommandResult>> Handle(RegistrarEntradaCommand request, CancellationToken cancellationToken)
    {
        var estoque = await _estoques.GetByIdAsync(request.EstoqueId, cancellationToken);
        if (estoque is null)
            return ResponseDefault<RegistrarEntradaCommandResult>.NotFound("Estoque não encontrado.");
        if (!estoque.Ativo)
            return ResponseDefault<RegistrarEntradaCommandResult>.Conflict("Estoque inativo.");

        var movimento = new EntradaProdutoEstoque
        {
            TenantId = _tenantContext.TenantId,
            EstoqueId = request.EstoqueId,
            ProdutoId = request.ProdutoId,
            Quantidade = request.Quantidade,
            QuantidadeRestante = request.Quantidade,
            CustoUnitario = request.CustoUnitario,
            Origem = request.Origem,
            Motivo = request.Motivo,
            FornecedorId = request.FornecedorId,
            DocumentoReferencia = request.DocumentoReferencia,
            DataMovimento = request.DataMovimento ?? DateTime.UtcNow,
            CreatedBy = _tenantContext.UserId
        };

        await _entradas.AddAsync(movimento, cancellationToken);

        // Garante linha de saldo, depois ajusta
        var saldoAtual = await _saldos.GetByEstoqueAndProdutoAsync(request.EstoqueId, request.ProdutoId, cancellationToken);
        if (saldoAtual is null)
        {
            saldoAtual = new EstoqueProduto
            {
                TenantId = _tenantContext.TenantId,
                EstoqueId = request.EstoqueId,
                ProdutoId = request.ProdutoId,
                SaldoTotal = request.Quantidade,
                SaldoReservado = 0,
                CreatedBy = _tenantContext.UserId
            };
            await _saldos.AddAsync(saldoAtual, cancellationToken);
        }
        else
        {
            await _saldos.AjustarSaldoAsync(request.EstoqueId, request.ProdutoId, request.Quantidade, 0, cancellationToken);
            saldoAtual.SaldoTotal += request.Quantidade;
        }

        return ResponseDefault<RegistrarEntradaCommandResult>.Created(
            new RegistrarEntradaCommandResult(
                movimento.Id,
                saldoAtual.SaldoTotal,
                saldoAtual.SaldoTotal - saldoAtual.SaldoReservado));
    }
}
