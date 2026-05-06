using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.CriarPlanoDeContas;

public sealed class CriarPlanoDeContasCommandHandler
    : IRequestHandler<CriarPlanoDeContasCommand, ResponseDefault<CriarPlanoDeContasCommandResult>>
{
    private readonly IPlanoDeContasRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarPlanoDeContasCommandHandler(IPlanoDeContasRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarPlanoDeContasCommandResult>> Handle(CriarPlanoDeContasCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repo.GetByCodigoAsync(request.Codigo, cancellationToken);
        if (existing is not null)
            return ResponseDefault<CriarPlanoDeContasCommandResult>.Conflict(
                $"Já existe uma conta com o código {request.Codigo}.");

        var nivel = 1;
        if (request.PaiId.HasValue)
        {
            var pai = await _repo.GetByIdAsync(request.PaiId.Value, cancellationToken);
            if (pai is null)
                return ResponseDefault<CriarPlanoDeContasCommandResult>.NotFound("Conta pai não encontrada.");
            if (pai.Tipo != request.Tipo)
                return ResponseDefault<CriarPlanoDeContasCommandResult>.Conflict(
                    "Tipo da conta filha deve ser igual ao do pai.");
            nivel = pai.Nivel + 1;
        }

        var conta = new Domain.Entities.Financeiro.PlanoDeContas
        {
            TenantId = _tenantContext.TenantId,
            Codigo = request.Codigo,
            Nome = request.Nome,
            Tipo = request.Tipo,
            PaiId = request.PaiId,
            Nivel = nivel,
            Aceita_Lancamento = request.AceitaLancamento,
            Ativo = true,
            CreatedBy = _tenantContext.UserId
        };

        await _repo.AddAsync(conta, cancellationToken);

        return ResponseDefault<CriarPlanoDeContasCommandResult>.Created(
            new CriarPlanoDeContasCommandResult(conta.Id, conta.Codigo, conta.Nome, conta.Nivel));
    }
}
