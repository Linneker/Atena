using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Command.RemoverLotacao;

public sealed class RemoverLotacaoCommandHandler
    : IRequestHandler<RemoverLotacaoCommand, ResponseDefault<RemoverLotacaoCommandResult>>
{
    private readonly ILotacaoRepository _repo;

    public RemoverLotacaoCommandHandler(ILotacaoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<RemoverLotacaoCommandResult>> Handle(
        RemoverLotacaoCommand request, CancellationToken cancellationToken)
    {
        var lotacao = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (lotacao is null)
            return ResponseDefault<RemoverLotacaoCommandResult>.NotFound(
                $"Lotação {request.Id} não encontrada.");

        await _repo.DeleteAsync(request.Id, cancellationToken);

        return ResponseDefault<RemoverLotacaoCommandResult>.Ok(
            new RemoverLotacaoCommandResult(request.Id));
    }
}
