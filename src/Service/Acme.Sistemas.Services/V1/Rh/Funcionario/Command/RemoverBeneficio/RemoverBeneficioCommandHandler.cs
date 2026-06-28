using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RemoverBeneficio;

public sealed class RemoverBeneficioCommandHandler
    : IRequestHandler<RemoverBeneficioCommand, ResponseDefault<RemoverBeneficioCommandResult>>
{
    private readonly IBeneficioFuncionarioRepository _repo;

    public RemoverBeneficioCommandHandler(IBeneficioFuncionarioRepository repo) => _repo = repo;

    public async Task<ResponseDefault<RemoverBeneficioCommandResult>> Handle(
        RemoverBeneficioCommand request, CancellationToken cancellationToken)
    {
        var vinculo = await _repo.GetByIdAsync(request.VinculoId, cancellationToken);
        if (vinculo is null)
            return ResponseDefault<RemoverBeneficioCommandResult>.NotFound(
                $"Vínculo de benefício {request.VinculoId} não encontrado.");

        await _repo.DeleteAsync(request.VinculoId, cancellationToken);

        return ResponseDefault<RemoverBeneficioCommandResult>.Ok(
            new RemoverBeneficioCommandResult(request.VinculoId));
    }
}
