using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Tenant.Command.ExcluirTenant;

public sealed class ExcluirTenantCommandHandler
    : IRequestHandler<ExcluirTenantCommand, ResponseDefault>
{
    private readonly ITenantRepository _repository;
    private readonly ITenantContext _tenantContext;

    public ExcluirTenantCommandHandler(ITenantRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault> Handle(ExcluirTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (tenant is null)
        {
            return ResponseDefault.BadRequest(Core.Response.Erros.Error.NotFound("Tenant não encontrado."));
        }

        var deletedBy = _tenantContext.UserId ?? Guid.Empty;
        await _repository.DeleteAsync(request.Id, deletedBy, cancellationToken);

        return ResponseDefault.NoContent();
    }
}
