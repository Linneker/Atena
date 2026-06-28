using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Query.ListarJornadas;

public sealed class ListarJornadasQueryHandler
    : IRequestHandler<ListarJornadasQuery, ResponseDefault<ListarJornadasQueryResult>>
{
    private readonly IJornadaRepository _repo;

    public ListarJornadasQueryHandler(IJornadaRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarJornadasQueryResult>> Handle(
        ListarJornadasQuery request, CancellationToken cancellationToken)
    {
        var jornadas = await _repo.ListAsync(request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountAsync(cancellationToken);

        var items = jornadas
            .Select(j => new ListarJornadasQueryItem(
                j.Id, j.Nome, j.Tipo, j.CargaSemanalHoras,
                j.CargaDiariaHoras, j.ToleranciaMinutos, j.Ativo))
            .ToList();

        return ResponseDefault<ListarJornadasQueryResult>.Ok(
            new ListarJornadasQueryResult(items, total));
    }
}
