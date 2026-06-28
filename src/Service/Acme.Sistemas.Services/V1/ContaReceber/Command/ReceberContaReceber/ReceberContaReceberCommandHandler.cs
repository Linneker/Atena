using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaReceber.Command.ReceberContaReceber;

public sealed class ReceberContaReceberCommandHandler
    : IRequestHandler<ReceberContaReceberCommand, ResponseDefault<ReceberContaReceberCommandResult>>
{
    private readonly IContaReceberRepository _repo;
    private readonly IReceitaRepository _receitas;
    private readonly ITenantContext _tenantContext;

    public ReceberContaReceberCommandHandler(
        IContaReceberRepository repo,
        IReceitaRepository receitas,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _receitas = receitas;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<ReceberContaReceberCommandResult>> Handle(ReceberContaReceberCommand request, CancellationToken cancellationToken)
    {
        var conta = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (conta is null)
            return ResponseDefault<ReceberContaReceberCommandResult>.NotFound("Conta a receber não encontrada.");

        if (conta.Status == StatusConta.Pago)
            return ResponseDefault<ReceberContaReceberCommandResult>.Conflict("Conta já recebida.");

        if (conta.Status == StatusConta.Cancelado)
            return ResponseDefault<ReceberContaReceberCommandResult>.Conflict("Conta cancelada não pode ser recebida.");

        if (request.ValorRecebido > conta.Saldo)
            return ResponseDefault<ReceberContaReceberCommandResult>.Conflict(
                $"Valor recebido ({request.ValorRecebido:C}) excede o saldo ({conta.Saldo:C}).");

        conta.ValorRecebido += request.ValorRecebido;
        conta.DataRecebimento = request.DataRecebimento;
        conta.ObservacaoRecebimento = request.Observacao;
        conta.UpdatedBy = _tenantContext.UserId;
        conta.Status = conta.ValorRecebido >= conta.ValorOriginal
            ? StatusConta.Pago
            : StatusConta.PagoParcial;

        await _repo.ReceberAsync(conta, cancellationToken);

        // Propagação para Receita.
        // - Se a conta veio de uma Receita prevista (ReceitaId != null), apenas atualiza essa receita.
        // - Se a conta não tem origem (gerada por Faturamento ou criada direto na conta),
        //   gera Receita Variável marcada como Paga para refletir no caixa.
        if (conta.ReceitaId.HasValue)
        {
            var receita = await _receitas.GetByIdAsync(conta.ReceitaId.Value, cancellationToken);
            if (receita is not null)
            {
                receita.ValorRecebido = (receita.ValorRecebido ?? 0) + request.ValorRecebido;
                receita.DataRecebimento = request.DataRecebimento;
                receita.ObservacaoRecebimento = request.Observacao;
                receita.UpdatedBy = _tenantContext.UserId;
                if (conta.Status == StatusConta.Pago)
                {
                    receita.StatusRecebimento = StatusPagamento.Pago;
                }
                await _receitas.ReceberAsync(receita, cancellationToken);
            }
        }
        else if (conta.Status == StatusConta.Pago)
        {
            var receita = new Domain.Entities.Financeiro.Receita
            {
                TenantId = _tenantContext.TenantId,
                Nome = conta.Descricao,
                Valor = conta.ValorOriginal,
                ReceitaFixa = false,
                DataPrevistaRecebimento = conta.DataVencimento,
                ClienteId = conta.ClienteId,
                StatusRecebimento = StatusPagamento.Pago,
                ValorRecebido = conta.ValorRecebido,
                DataRecebimento = request.DataRecebimento,
                ObservacaoRecebimento = request.Observacao,
                CreatedBy = _tenantContext.UserId
            };
            await _receitas.AddAsync(receita, cancellationToken);
        }

        return ResponseDefault<ReceberContaReceberCommandResult>.Ok(
            new ReceberContaReceberCommandResult(conta.Id, conta.Status, conta.ValorRecebido, conta.Saldo));
    }
}
