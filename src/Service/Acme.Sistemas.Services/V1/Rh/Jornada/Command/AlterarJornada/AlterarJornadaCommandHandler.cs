using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Command.AlterarJornada;

public sealed class AlterarJornadaCommandHandler
    : IRequestHandler<AlterarJornadaCommand, ResponseDefault<AlterarJornadaCommandResult>>
{
    private readonly IJornadaRepository _repo;
    private readonly ITenantContext _tenantContext;

    public AlterarJornadaCommandHandler(IJornadaRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarJornadaCommandResult>> Handle(
        AlterarJornadaCommand request, CancellationToken cancellationToken)
    {
        var jornada = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (jornada is null)
            return ResponseDefault<AlterarJornadaCommandResult>.NotFound(
                $"Jornada {request.Id} não encontrada.");

        // Se mudou o nome, validar unicidade contra outras jornadas do tenant.
        if (!string.Equals(jornada.Nome, request.Nome, StringComparison.OrdinalIgnoreCase))
        {
            var conflito = await _repo.GetByNomeAsync(request.Nome, cancellationToken);
            if (conflito is not null && conflito.Id != request.Id)
                return ResponseDefault<AlterarJornadaCommandResult>.Conflict(
                    $"Já existe uma jornada com o nome '{request.Nome}'.");
        }

        jornada.Nome = request.Nome;
        jornada.Tipo = request.Tipo;
        jornada.CargaSemanalHoras = request.CargaSemanalHoras;
        jornada.CargaDiariaHoras = request.CargaDiariaHoras;
        jornada.JanelasJson = request.JanelasJson;
        jornada.PermiteMarcarIntervalo = request.PermiteMarcarIntervalo;
        jornada.ToleranciaMinutos = request.ToleranciaMinutos;
        jornada.Ativo = request.Ativo;
        jornada.UpdatedBy = _tenantContext.UserId;

        await _repo.UpdateAsync(jornada, cancellationToken);

        return ResponseDefault<AlterarJornadaCommandResult>.Ok(
            new AlterarJornadaCommandResult(jornada.Id));
    }
}
