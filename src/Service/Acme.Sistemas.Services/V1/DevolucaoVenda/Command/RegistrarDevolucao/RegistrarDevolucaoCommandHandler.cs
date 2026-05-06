using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Entities.Vendas;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Faturamento.Events;
using DevolucaoEntity = Acme.Sistemas.Domain.Entities.Vendas.DevolucaoVenda;

namespace Acme.Sistemas.Services.V1.DevolucaoVenda.Command.RegistrarDevolucao;

public sealed class RegistrarDevolucaoCommandHandler
    : IRequestHandler<RegistrarDevolucaoCommand, ResponseDefault<RegistrarDevolucaoCommandResult>>
{
    private readonly IDevolucaoVendaRepository _devolucoes;
    private readonly IFaturamentoRepository _faturamentos;
    private readonly IEntradaProdutoEstoqueRepository _entradas;
    private readonly IEstoqueProdutoRepository _saldos;
    private readonly IContaReceberRepository _contasReceber;
    private readonly IMediator _mediator;
    private readonly ITenantContext _tenantContext;

    public RegistrarDevolucaoCommandHandler(
        IDevolucaoVendaRepository devolucoes,
        IFaturamentoRepository faturamentos,
        IEntradaProdutoEstoqueRepository entradas,
        IEstoqueProdutoRepository saldos,
        IContaReceberRepository contasReceber,
        IMediator mediator,
        ITenantContext tenantContext)
    {
        _devolucoes = devolucoes;
        _faturamentos = faturamentos;
        _entradas = entradas;
        _saldos = saldos;
        _contasReceber = contasReceber;
        _mediator = mediator;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<RegistrarDevolucaoCommandResult>> Handle(RegistrarDevolucaoCommand request, CancellationToken cancellationToken)
    {
        var faturamento = await _faturamentos.GetByIdAsync(request.FaturamentoId, cancellationToken);
        if (faturamento is null)
            return ResponseDefault<RegistrarDevolucaoCommandResult>.NotFound("Faturamento não encontrado.");

        var fatItens = (await _faturamentos.ListItensAsync(faturamento.Id, cancellationToken))
            .ToDictionary(i => i.Id);

        decimal valorDevolvido = 0;
        var devItens = new List<DevolucaoVendaItem>();

        foreach (var item in request.Itens)
        {
            if (!fatItens.TryGetValue(item.FaturamentoItemId, out var fi))
                return ResponseDefault<RegistrarDevolucaoCommandResult>.BadRequest(
                    Core.Response.Erros.Error.Validation($"Item {item.FaturamentoItemId} não pertence ao faturamento."));
            if (item.Quantidade > fi.Quantidade)
                return ResponseDefault<RegistrarDevolucaoCommandResult>.Conflict(
                    $"Quantidade ({item.Quantidade}) excede a faturada ({fi.Quantidade}).");

            devItens.Add(new DevolucaoVendaItem
            {
                TenantId = _tenantContext.TenantId,
                FaturamentoItemId = fi.Id,
                ProdutoId = fi.ProdutoId,
                Quantidade = item.Quantidade,
                PrecoUnitario = fi.PrecoUnitario,
                CreatedBy = _tenantContext.UserId
            });
            valorDevolvido += item.Quantidade * fi.PrecoUnitario;
        }

        var totalQtdFat = fatItens.Values.Sum(i => i.Quantidade);
        var totalQtdDev = request.Itens.Sum(i => i.Quantidade);
        var tipo = totalQtdDev >= totalQtdFat ? TipoDevolucao.Total : TipoDevolucao.Parcial;

        var devolucao = new DevolucaoEntity
        {
            TenantId = _tenantContext.TenantId,
            FaturamentoId = faturamento.Id,
            DataDevolucao = DateTime.UtcNow,
            Tipo = tipo,
            ValorTotal = valorDevolvido,
            Motivo = request.Motivo,
            CreatedBy = _tenantContext.UserId
        };
        await _devolucoes.AddAsync(devolucao, cancellationToken);
        foreach (var di in devItens) di.DevolucaoVendaId = devolucao.Id;
        await _devolucoes.AddItensAsync(devItens, cancellationToken);

        // Retorna estoque (entrada por devolução)
        foreach (var item in request.Itens)
        {
            var fi = fatItens[item.FaturamentoItemId];
            await _entradas.AddAsync(new EntradaProdutoEstoque
            {
                TenantId = _tenantContext.TenantId,
                EstoqueId = request.EstoqueDestinoId,
                ProdutoId = fi.ProdutoId,
                Quantidade = item.Quantidade,
                QuantidadeRestante = item.Quantidade,
                CustoUnitario = fi.PrecoUnitario,
                Origem = OrigemMovimento.Devolucao,
                Motivo = $"Devolução do faturamento {faturamento.Numero}",
                DocumentoReferencia = devolucao.Id.ToString(),
                DataMovimento = devolucao.DataDevolucao,
                CreatedBy = _tenantContext.UserId
            }, cancellationToken);

            var saldo = await _saldos.GetByEstoqueAndProdutoAsync(request.EstoqueDestinoId, fi.ProdutoId, cancellationToken);
            if (saldo is null)
            {
                await _saldos.AddAsync(new EstoqueProduto
                {
                    TenantId = _tenantContext.TenantId,
                    EstoqueId = request.EstoqueDestinoId,
                    ProdutoId = fi.ProdutoId,
                    SaldoTotal = item.Quantidade,
                    CreatedBy = _tenantContext.UserId
                }, cancellationToken);
            }
            else
            {
                await _saldos.AjustarSaldoAsync(request.EstoqueDestinoId, fi.ProdutoId, item.Quantidade, 0, cancellationToken);
            }
        }

        // Estorna conta a receber (se ainda pendente, cancela; se paga, deixa para usuário tratar)
        bool estornada = false;
        if (faturamento.ContaReceberId.HasValue)
        {
            var conta = await _contasReceber.GetByIdAsync(faturamento.ContaReceberId.Value, cancellationToken);
            if (conta is not null && conta.Status == StatusConta.Pendente)
            {
                conta.Status = StatusConta.Cancelado;
                conta.UpdatedBy = _tenantContext.UserId;
                await _contasReceber.UpdateAsync(conta, cancellationToken);
                estornada = true;
            }
        }

        // Dispara NF-e de devolução
        await _mediator.Publish(new NFeEmissaoSolicitadaNotification(
            _tenantContext.TenantId, devolucao.Id, NFeOperacao.Devolucao, valorDevolvido, DateTime.UtcNow),
            cancellationToken);

        return ResponseDefault<RegistrarDevolucaoCommandResult>.Created(
            new RegistrarDevolucaoCommandResult(devolucao.Id, valorDevolvido, estornada, NFeDevolucaoSolicitada: true));
    }
}
