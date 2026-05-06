using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaReceber.Command.CriarContaReceber;

public sealed class CriarContaReceberCommandHandler
    : IRequestHandler<CriarContaReceberCommand, ResponseDefault<CriarContaReceberCommandResult>>
{
    private readonly IContaReceberRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarContaReceberCommandHandler(IContaReceberRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarContaReceberCommandResult>> Handle(CriarContaReceberCommand request, CancellationToken cancellationToken)
    {
        var conta = new Domain.Entities.Financeiro.ContaReceber
        {
            TenantId = _tenantContext.TenantId,
            Descricao = request.Descricao,
            ClienteId = request.ClienteId,
            ReceitaId = request.ReceitaId,
            PlanoDeContasId = request.PlanoDeContasId,
            ValorOriginal = request.ValorOriginal,
            DataVencimento = request.DataVencimento,
            Status = StatusConta.Pendente,
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(conta, cancellationToken);

        return ResponseDefault<CriarContaReceberCommandResult>.Created(
            new CriarContaReceberCommandResult(conta.Id, conta.Descricao, conta.ValorOriginal, conta.DataVencimento));
    }
}
