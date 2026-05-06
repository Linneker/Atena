using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaPagar.Command.CriarContaPagar;

public sealed class CriarContaPagarCommandHandler
    : IRequestHandler<CriarContaPagarCommand, ResponseDefault<CriarContaPagarCommandResult>>
{
    private readonly IContaPagarRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarContaPagarCommandHandler(IContaPagarRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarContaPagarCommandResult>> Handle(CriarContaPagarCommand request, CancellationToken cancellationToken)
    {
        var conta = new Domain.Entities.Financeiro.ContaPagar
        {
            TenantId = _tenantContext.TenantId,
            Descricao = request.Descricao,
            FornecedorId = request.FornecedorId,
            DespesaId = request.DespesaId,
            PlanoDeContasId = request.PlanoDeContasId,
            ValorOriginal = request.ValorOriginal,
            DataVencimento = request.DataVencimento,
            Status = StatusConta.Pendente,
            Observacao = request.Observacao,
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(conta, cancellationToken);

        return ResponseDefault<CriarContaPagarCommandResult>.Created(
            new CriarContaPagarCommandResult(conta.Id, conta.Descricao, conta.ValorOriginal, conta.DataVencimento));
    }
}
