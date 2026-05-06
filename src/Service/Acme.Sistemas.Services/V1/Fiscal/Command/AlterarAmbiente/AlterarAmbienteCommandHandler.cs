using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.AlterarAmbiente;

public sealed class AlterarAmbienteCommandHandler
    : IRequestHandler<AlterarAmbienteCommand, ResponseDefault<AlterarAmbienteCommandResult>>
{
    private readonly IConfiguracaoFiscalRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarAmbienteCommandHandler(IConfiguracaoFiscalRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarAmbienteCommandResult>> Handle(AlterarAmbienteCommand request, CancellationToken cancellationToken)
    {
        var config = await _repo.GetAsync(cancellationToken);
        if (config is null)
            return ResponseDefault<AlterarAmbienteCommandResult>.NotFound(
                "Configuração fiscal não encontrada. Importe um certificado primeiro.");

        config.Ambiente = request.Ambiente;
        config.UpdatedBy = _tenantContext.UserId;
        await _repo.UpsertAsync(config, cancellationToken);

        return ResponseDefault<AlterarAmbienteCommandResult>.Ok(
            new AlterarAmbienteCommandResult(config.Ambiente, DateTime.UtcNow));
    }
}
