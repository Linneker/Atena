using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.AlterarPlanoDeContas;

public sealed class AlterarPlanoDeContasCommandHandler
    : IRequestHandler<AlterarPlanoDeContasCommand, ResponseDefault<AlterarPlanoDeContasCommandResult>>
{
    private readonly IPlanoDeContasRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarPlanoDeContasCommandHandler(IPlanoDeContasRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarPlanoDeContasCommandResult>> Handle(AlterarPlanoDeContasCommand request, CancellationToken cancellationToken)
    {
        var conta = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (conta is null)
            return ResponseDefault<AlterarPlanoDeContasCommandResult>.NotFound("Conta não encontrada.");

        // Se a conta tem filhas, não pode aceitar lançamento
        if (request.AceitaLancamento && await _repo.HasFilhosAsync(conta.Id, cancellationToken))
            return ResponseDefault<AlterarPlanoDeContasCommandResult>.Conflict(
                "Conta com filhas não pode aceitar lançamento (apenas folhas).");

        conta.Nome = request.Nome;
        conta.Aceita_Lancamento = request.AceitaLancamento;
        conta.Ativo = request.Ativo;
        conta.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(conta, cancellationToken);
        return ResponseDefault<AlterarPlanoDeContasCommandResult>.Ok(new AlterarPlanoDeContasCommandResult(conta.Id));
    }
}
