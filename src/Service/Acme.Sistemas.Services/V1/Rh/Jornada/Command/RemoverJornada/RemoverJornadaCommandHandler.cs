using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Command.RemoverJornada;

public sealed class RemoverJornadaCommandHandler
    : IRequestHandler<RemoverJornadaCommand, ResponseDefault<RemoverJornadaCommandResult>>
{
    private readonly IJornadaRepository _repo;

    public RemoverJornadaCommandHandler(IJornadaRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<RemoverJornadaCommandResult>> Handle(
        RemoverJornadaCommand request, CancellationToken cancellationToken)
    {
        var jornada = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (jornada is null)
            return ResponseDefault<RemoverJornadaCommandResult>.NotFound(
                $"Jornada {request.Id} não encontrada.");

        // Soft delete: BaseRepository.DeleteAsync seta deleted_at.
        // Escalas_funcionario apontam para jornada_id sem ON DELETE CASCADE; manter como
        // delete lógico permite reativar e preserva histórico de escalas vigentes.
        await _repo.DeleteAsync(request.Id, cancellationToken);

        return ResponseDefault<RemoverJornadaCommandResult>.Ok(
            new RemoverJornadaCommandResult(request.Id));
    }
}
