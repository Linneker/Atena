using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.RemoverDepartamento;

public sealed class RemoverDepartamentoCommandHandler
    : IRequestHandler<RemoverDepartamentoCommand, ResponseDefault<RemoverDepartamentoCommandResult>>
{
    private readonly IDepartamentoRepository _repo;

    public RemoverDepartamentoCommandHandler(IDepartamentoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<RemoverDepartamentoCommandResult>> Handle(
        RemoverDepartamentoCommand request, CancellationToken cancellationToken)
    {
        var depto = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (depto is null)
            return ResponseDefault<RemoverDepartamentoCommandResult>.NotFound(
                $"Departamento {request.Id} não encontrado.");

        await _repo.DeleteAsync(request.Id, cancellationToken);

        return ResponseDefault<RemoverDepartamentoCommandResult>.Ok(
            new RemoverDepartamentoCommandResult(request.Id));
    }
}
