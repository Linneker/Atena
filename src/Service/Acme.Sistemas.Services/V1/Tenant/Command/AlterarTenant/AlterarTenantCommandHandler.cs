using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Tenant.Command.AlterarTenant;

public sealed class AlterarTenantCommandHandler
    : IRequestHandler<AlterarTenantCommand, ResponseDefault<AlterarTenantCommandResult>>
{
    private readonly ITenantRepository _repository;
    private readonly ITenantContext _tenantContext;

    public AlterarTenantCommandHandler(ITenantRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarTenantCommandResult>> Handle(
        AlterarTenantCommand request,
        CancellationToken cancellationToken)
    {
        var tenant = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (tenant is null)
        {
            return ResponseDefault<AlterarTenantCommandResult>.NotFound("Tenant não encontrado.");
        }

        tenant.RazaoSocial = request.RazaoSocial;
        tenant.Plano = request.Plano;
        tenant.Status = (StatusAtivo)request.Status;
        tenant.LogoUrl = request.LogoUrl;
        tenant.CorPrimaria = request.CorPrimaria;
        tenant.FusoHorario = request.FusoHorario;
        tenant.UpdatedBy = _tenantContext.UserId;

        await _repository.UpdateAsync(tenant, cancellationToken);

        return ResponseDefault<AlterarTenantCommandResult>.Ok(new AlterarTenantCommandResult(tenant.Id));
    }
}
