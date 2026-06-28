using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ObterSaldo;

public sealed class ObterSaldoQueryHandler
    : IRequestHandler<ObterSaldoQuery, ResponseDefault<ObterSaldoQueryResult>>
{
    private readonly IBancoHorasSaldoRepository _repo;

    public ObterSaldoQueryHandler(IBancoHorasSaldoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ObterSaldoQueryResult>> Handle(
        ObterSaldoQuery request, CancellationToken cancellationToken)
    {
        var s = await _repo.GetByFuncionarioCompetenciaAsync(
            request.FuncionarioId, request.Competencia, cancellationToken);

        // Sem saldo cadastrado = zero
        if (s is null)
            return ResponseDefault<ObterSaldoQueryResult>.Ok(
                new ObterSaldoQueryResult(request.FuncionarioId, request.Competencia, 0, 0, 0, null));

        return ResponseDefault<ObterSaldoQueryResult>.Ok(new ObterSaldoQueryResult(
            s.FuncionarioId, s.Competencia, s.HorasDevidas, s.HorasRealizadas, s.SaldoMinutos, s.PoliticaId));
    }
}
