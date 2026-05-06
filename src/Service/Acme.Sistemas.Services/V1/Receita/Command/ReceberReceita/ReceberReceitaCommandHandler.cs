using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Receita.Command.ReceberReceita;

public sealed class ReceberReceitaCommandHandler
    : IRequestHandler<ReceberReceitaCommand, ResponseDefault<ReceberReceitaCommandResult>>
{
    private readonly IReceitaRepository _receitas;
    private readonly ITenantContext _tenantContext;

    public ReceberReceitaCommandHandler(IReceitaRepository receitas, ITenantContext tenantContext)
    {
        _receitas = receitas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<ReceberReceitaCommandResult>> Handle(
        ReceberReceitaCommand request,
        CancellationToken cancellationToken)
    {
        var receita = await _receitas.GetByIdAsync(request.Id, cancellationToken);
        if (receita is null)
        {
            return ResponseDefault<ReceberReceitaCommandResult>.NotFound("Receita não encontrada.");
        }

        if (receita.StatusRecebimento == StatusPagamento.Pago)
        {
            return ResponseDefault<ReceberReceitaCommandResult>.Conflict("Receita já está recebida.");
        }

        if (receita.StatusRecebimento == StatusPagamento.Cancelado)
        {
            return ResponseDefault<ReceberReceitaCommandResult>.Conflict(
                "Receita cancelada não pode ser recebida.");
        }

        receita.StatusRecebimento = StatusPagamento.Pago;
        receita.ValorRecebido = request.ValorRecebido;
        receita.DataRecebimento = request.DataRecebimento;
        receita.FormaPagamento = request.FormaPagamento;
        receita.ObservacaoRecebimento = request.Observacao;
        receita.UpdatedBy = _tenantContext.UserId;

        await _receitas.ReceberAsync(receita, cancellationToken);

        return ResponseDefault<ReceberReceitaCommandResult>.Ok(
            new ReceberReceitaCommandResult(
                receita.Id,
                receita.StatusRecebimento,
                receita.ValorRecebido.Value,
                receita.DataRecebimento.Value));
    }
}
