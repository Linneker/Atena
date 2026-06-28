using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Query.ObterJornada;

public sealed class ObterJornadaQueryHandler
    : IRequestHandler<ObterJornadaQuery, ResponseDefault<ObterJornadaQueryResult>>
{
    private readonly IJornadaRepository _repo;

    public ObterJornadaQueryHandler(IJornadaRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ObterJornadaQueryResult>> Handle(
        ObterJornadaQuery request, CancellationToken cancellationToken)
    {
        var j = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (j is null)
            return ResponseDefault<ObterJornadaQueryResult>.NotFound($"Jornada {request.Id} não encontrada.");

        return ResponseDefault<ObterJornadaQueryResult>.Ok(new ObterJornadaQueryResult(
            j.Id, j.Nome, j.Tipo, j.CargaSemanalHoras, j.CargaDiariaHoras,
            j.JanelasJson, j.PermiteMarcarIntervalo, j.ToleranciaMinutos, j.Ativo));
    }
}
