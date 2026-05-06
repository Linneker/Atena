using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaPagar.Command.BaixarContaPagar;

public sealed class BaixarContaPagarCommandHandler
    : IRequestHandler<BaixarContaPagarCommand, ResponseDefault<BaixarContaPagarCommandResult>>
{
    private readonly IContaPagarRepository _repo;
    private readonly IPagamentoRepository _pagamentos;
    private readonly ITenantContext _tenantContext;

    public BaixarContaPagarCommandHandler(
        IContaPagarRepository repo,
        IPagamentoRepository pagamentos,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _pagamentos = pagamentos;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<BaixarContaPagarCommandResult>> Handle(BaixarContaPagarCommand request, CancellationToken cancellationToken)
    {
        var conta = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (conta is null)
            return ResponseDefault<BaixarContaPagarCommandResult>.NotFound("Conta a pagar não encontrada.");

        if (conta.Status == StatusConta.Pago)
            return ResponseDefault<BaixarContaPagarCommandResult>.Conflict("Conta já está paga.");

        if (conta.Status == StatusConta.Cancelado)
            return ResponseDefault<BaixarContaPagarCommandResult>.Conflict("Conta cancelada não pode ser baixada.");

        var saldoAnterior = conta.Saldo;
        if (request.ValorPago > saldoAnterior)
            return ResponseDefault<BaixarContaPagarCommandResult>.Conflict(
                $"Valor pago ({request.ValorPago:C}) excede o saldo da conta ({saldoAnterior:C}).");

        conta.ValorPago += request.ValorPago;
        conta.DataPagamento = request.DataPagamento;
        conta.UpdatedBy = _tenantContext.UserId;
        conta.Status = conta.ValorPago >= conta.ValorOriginal
            ? StatusConta.Pago
            : StatusConta.PagoParcial;

        await _repo.BaixarAsync(conta, cancellationToken);

        await _pagamentos.AddAsync(new Pagamento
        {
            TenantId = _tenantContext.TenantId,
            ContaPagarId = conta.Id,
            Valor = request.ValorPago,
            DataPagamento = request.DataPagamento,
            FormaPagamento = request.FormaPagamento,
            Observacao = request.Observacao,
            CreatedBy = _tenantContext.UserId
        }, cancellationToken);

        return ResponseDefault<BaixarContaPagarCommandResult>.Ok(
            new BaixarContaPagarCommandResult(conta.Id, conta.Status, conta.ValorPago, conta.Saldo));
    }
}
