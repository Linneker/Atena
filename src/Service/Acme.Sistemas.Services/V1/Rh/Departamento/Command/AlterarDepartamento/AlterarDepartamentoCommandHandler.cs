using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.AlterarDepartamento;

public sealed class AlterarDepartamentoCommandHandler
    : IRequestHandler<AlterarDepartamentoCommand, ResponseDefault<AlterarDepartamentoCommandResult>>
{
    private readonly IDepartamentoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarDepartamentoCommandHandler(IDepartamentoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarDepartamentoCommandResult>> Handle(
        AlterarDepartamentoCommand request, CancellationToken cancellationToken)
    {
        var depto = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (depto is null)
            return ResponseDefault<AlterarDepartamentoCommandResult>.NotFound(
                $"Departamento {request.Id} não encontrado.");

        if (!string.IsNullOrWhiteSpace(request.Codigo) &&
            !string.Equals(depto.Codigo, request.Codigo, StringComparison.OrdinalIgnoreCase))
        {
            var conflito = await _repo.GetByCodigoAsync(request.Codigo, cancellationToken);
            if (conflito is not null && conflito.Id != request.Id)
                return ResponseDefault<AlterarDepartamentoCommandResult>.Conflict(
                    $"Já existe um departamento com o código '{request.Codigo}'.");
        }

        depto.Codigo = request.Codigo;
        depto.Nome = request.Nome;
        depto.CentroDeCustoId = request.CentroDeCustoId;
        depto.Ativo = request.Ativo;
        depto.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(depto, cancellationToken);

        return ResponseDefault<AlterarDepartamentoCommandResult>.Ok(
            new AlterarDepartamentoCommandResult(depto.Id));
    }
}
