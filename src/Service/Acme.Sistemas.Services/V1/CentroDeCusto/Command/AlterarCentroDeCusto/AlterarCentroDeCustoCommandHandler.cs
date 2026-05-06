using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.AlterarCentroDeCusto;

public sealed class AlterarCentroDeCustoCommandHandler
    : IRequestHandler<AlterarCentroDeCustoCommand, ResponseDefault<AlterarCentroDeCustoCommandResult>>
{
    private readonly ICentroDeCustoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarCentroDeCustoCommandHandler(ICentroDeCustoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarCentroDeCustoCommandResult>> Handle(AlterarCentroDeCustoCommand request, CancellationToken cancellationToken)
    {
        var centro = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (centro is null)
            return ResponseDefault<AlterarCentroDeCustoCommandResult>.NotFound("Centro de custo não encontrado.");

        centro.Nome = request.Nome;
        centro.Descricao = request.Descricao;
        centro.ResponsavelId = request.ResponsavelId;
        centro.Ativo = request.Ativo;
        centro.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(centro, cancellationToken);
        return ResponseDefault<AlterarCentroDeCustoCommandResult>.Ok(new AlterarCentroDeCustoCommandResult(centro.Id));
    }
}
