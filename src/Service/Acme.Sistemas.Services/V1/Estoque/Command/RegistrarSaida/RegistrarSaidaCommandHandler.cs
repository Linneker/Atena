using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Estoque.Events;
using Acme.Sistemas.Services.V1.Estoque.Services;

namespace Acme.Sistemas.Services.V1.Estoque.Command.RegistrarSaida;

public sealed class RegistrarSaidaCommandHandler
    : IRequestHandler<RegistrarSaidaCommand, ResponseDefault<RegistrarSaidaCommandResult>>
{
    private readonly IEstoqueRepository _estoques;
    private readonly IEstoqueProdutoRepository _saldos;
    private readonly ISaidaProdutoEstoqueRepository _saidas;
    private readonly IProdutoRepository _produtos;
    private readonly FifoCustoCalculator _fifo;
    private readonly IMediator _mediator;
    private readonly ITenantContext _tenantContext;

    public RegistrarSaidaCommandHandler(
        IEstoqueRepository estoques,
        IEstoqueProdutoRepository saldos,
        ISaidaProdutoEstoqueRepository saidas,
        IProdutoRepository produtos,
        FifoCustoCalculator fifo,
        IMediator mediator,
        ITenantContext tenantContext)
    {
        _estoques = estoques;
        _saldos = saldos;
        _saidas = saidas;
        _produtos = produtos;
        _fifo = fifo;
        _mediator = mediator;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<RegistrarSaidaCommandResult>> Handle(RegistrarSaidaCommand request, CancellationToken cancellationToken)
    {
        var estoque = await _estoques.GetByIdAsync(request.EstoqueId, cancellationToken);
        if (estoque is null)
            return ResponseDefault<RegistrarSaidaCommandResult>.NotFound("Estoque não encontrado.");
        if (!estoque.Ativo)
            return ResponseDefault<RegistrarSaidaCommandResult>.Conflict("Estoque inativo.");

        var saldoAtual = await _saldos.GetByEstoqueAndProdutoAsync(request.EstoqueId, request.ProdutoId, cancellationToken);
        var disponivelAtual = saldoAtual?.SaldoDisponivel ?? 0;

        if (request.Quantidade > disponivelAtual && !estoque.PermiteSaldoNegativo)
        {
            return ResponseDefault<RegistrarSaidaCommandResult>.Conflict(
                $"Saldo disponível insuficiente. Disponível: {disponivelAtual}, solicitado: {request.Quantidade}. " +
                "O estoque não permite saldo negativo.");
        }

        // FIFO: consome lotes e calcula CMV médio ponderado
        var fifoResult = await _fifo.ConsumirAsync(
            request.EstoqueId, request.ProdutoId, request.Quantidade, cancellationToken);

        var movimento = new SaidaProdutoEstoque
        {
            TenantId = _tenantContext.TenantId,
            EstoqueId = request.EstoqueId,
            ProdutoId = request.ProdutoId,
            Quantidade = request.Quantidade,
            CustoUnitario = request.CustoUnitario,
            CmvUnitario = fifoResult.CmvUnitarioMedio,
            Origem = request.Origem,
            Motivo = request.Motivo,
            ClienteId = request.ClienteId,
            DocumentoReferencia = request.DocumentoReferencia,
            DataMovimento = request.DataMovimento ?? DateTime.UtcNow,
            CreatedBy = _tenantContext.UserId
        };

        await _saidas.AddAsync(movimento, cancellationToken);

        if (saldoAtual is null)
        {
            saldoAtual = new EstoqueProduto
            {
                TenantId = _tenantContext.TenantId,
                EstoqueId = request.EstoqueId,
                ProdutoId = request.ProdutoId,
                SaldoTotal = -request.Quantidade,
                SaldoReservado = 0,
                CreatedBy = _tenantContext.UserId
            };
            await _saldos.AddAsync(saldoAtual, cancellationToken);
        }
        else
        {
            await _saldos.AjustarSaldoAsync(request.EstoqueId, request.ProdutoId, -request.Quantidade, 0, cancellationToken);
            saldoAtual.SaldoTotal -= request.Quantidade;
        }

        var novoDisponivel = saldoAtual.SaldoTotal - saldoAtual.SaldoReservado;

        // Alerta de estoque mínimo
        var produto = await _produtos.GetByIdAsync(request.ProdutoId, cancellationToken);
        if (produto?.EstoqueMinimo is decimal minimo && novoDisponivel <= minimo)
        {
            await _mediator.Publish(
                new AlertaEstoqueMinimoNotification(
                    _tenantContext.TenantId,
                    request.EstoqueId,
                    request.ProdutoId,
                    novoDisponivel,
                    minimo,
                    DateTime.UtcNow),
                cancellationToken);
        }

        return ResponseDefault<RegistrarSaidaCommandResult>.Created(
            new RegistrarSaidaCommandResult(
                movimento.Id,
                saldoAtual.SaldoTotal,
                novoDisponivel));
    }
}
