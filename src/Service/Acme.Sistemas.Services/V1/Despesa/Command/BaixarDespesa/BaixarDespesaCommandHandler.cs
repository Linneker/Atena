using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Despesa.Command.BaixarDespesa;

public sealed class BaixarDespesaCommandHandler
    : IRequestHandler<BaixarDespesaCommand, ResponseDefault<BaixarDespesaCommandResult>>
{
    private readonly IDespesaRepository _despesas;
    private readonly ITenantContext _tenantContext;

    public BaixarDespesaCommandHandler(IDespesaRepository despesas, ITenantContext tenantContext)
    {
        _despesas = despesas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<BaixarDespesaCommandResult>> Handle(
        BaixarDespesaCommand request,
        CancellationToken cancellationToken)
    {
        var despesa = await _despesas.GetByIdAsync(request.Id, cancellationToken);
        if (despesa is null)
        {
            return ResponseDefault<BaixarDespesaCommandResult>.NotFound("Despesa não encontrada.");
        }

        if (despesa.StatusPagamento == StatusPagamento.Pago)
        {
            return ResponseDefault<BaixarDespesaCommandResult>.Conflict(
                "Despesa já está paga.");
        }

        if (despesa.StatusPagamento == StatusPagamento.Cancelado)
        {
            return ResponseDefault<BaixarDespesaCommandResult>.Conflict(
                "Despesa cancelada não pode ser baixada.");
        }

        despesa.StatusPagamento = StatusPagamento.Pago;
        despesa.ValorPago = request.ValorPago;
        despesa.DataPagamento = request.DataPagamento;
        despesa.FormaPagamento = request.FormaPagamento;
        despesa.ObservacaoPagamento = request.Observacao;
        despesa.UpdatedBy = _tenantContext.UserId;

        await _despesas.BaixarAsync(despesa, cancellationToken);

        return ResponseDefault<BaixarDespesaCommandResult>.Ok(
            new BaixarDespesaCommandResult(
                despesa.Id,
                despesa.StatusPagamento,
                despesa.ValorPago.Value,
                despesa.DataPagamento.Value));
    }
}
