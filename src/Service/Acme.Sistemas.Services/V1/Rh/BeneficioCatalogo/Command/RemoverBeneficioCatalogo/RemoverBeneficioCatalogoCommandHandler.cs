using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.RemoverBeneficioCatalogo;

public sealed class RemoverBeneficioCatalogoCommandHandler
    : IRequestHandler<RemoverBeneficioCatalogoCommand, ResponseDefault<RemoverBeneficioCatalogoCommandResult>>
{
    private readonly IBeneficioCatalogoRepository _repo;

    public RemoverBeneficioCatalogoCommandHandler(IBeneficioCatalogoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<RemoverBeneficioCatalogoCommandResult>> Handle(
        RemoverBeneficioCatalogoCommand request, CancellationToken cancellationToken)
    {
        var benef = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (benef is null)
            return ResponseDefault<RemoverBeneficioCatalogoCommandResult>.NotFound(
                $"Benefício {request.Id} não encontrado.");

        await _repo.DeleteAsync(request.Id, cancellationToken);

        return ResponseDefault<RemoverBeneficioCatalogoCommandResult>.Ok(
            new RemoverBeneficioCatalogoCommandResult(request.Id));
    }
}
