using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaReceber.Command.ReceberContaReceber;

public sealed class ReceberContaReceberCommandHandler
    : IRequestHandler<ReceberContaReceberCommand, ResponseDefault<ReceberContaReceberCommandResult>>
{
    private readonly IContaReceberRepository _repo;
    private readonly ITenantContext _tenantContext;

    public ReceberContaReceberCommandHandler(IContaReceberRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
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

        return ResponseDefault<ReceberContaReceberCommandResult>.Ok(
            new ReceberContaReceberCommandResult(conta.Id, conta.Status, conta.ValorRecebido, conta.Saldo));
    }
}
