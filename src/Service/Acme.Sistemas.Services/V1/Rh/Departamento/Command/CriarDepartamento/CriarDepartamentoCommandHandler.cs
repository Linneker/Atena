using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using DepartamentoEntity = Acme.Sistemas.Domain.Entities.Rh.Departamento;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.CriarDepartamento;

public sealed class CriarDepartamentoCommandHandler
    : IRequestHandler<CriarDepartamentoCommand, ResponseDefault<CriarDepartamentoCommandResult>>
{
    private readonly IDepartamentoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public CriarDepartamentoCommandHandler(IDepartamentoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarDepartamentoCommandResult>> Handle(
        CriarDepartamentoCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.Codigo))
        {
            var existente = await _repo.GetByCodigoAsync(request.Codigo, cancellationToken);
            if (existente is not null)
                return ResponseDefault<CriarDepartamentoCommandResult>.Conflict(
                    $"Já existe um departamento com o código '{request.Codigo}'.");
        }

        var depto = new DepartamentoEntity
        {
            TenantId = _tenantContext.TenantId,
            Codigo = request.Codigo,
            Nome = request.Nome,
            CentroDeCustoId = request.CentroDeCustoId,
            Ativo = true,
            CreatedBy = _tenantContext.UserId,
        };

        await _repo.AddAsync(depto, cancellationToken);

        return ResponseDefault<CriarDepartamentoCommandResult>.Created(
            new CriarDepartamentoCommandResult(depto.Id, depto.Nome));
    }
}
