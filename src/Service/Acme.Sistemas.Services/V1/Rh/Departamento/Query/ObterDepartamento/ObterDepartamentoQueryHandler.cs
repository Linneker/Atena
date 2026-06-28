using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Query.ObterDepartamento;

public sealed class ObterDepartamentoQueryHandler
    : IRequestHandler<ObterDepartamentoQuery, ResponseDefault<ObterDepartamentoQueryResult>>
{
    private readonly IDepartamentoRepository _repo;

    public ObterDepartamentoQueryHandler(IDepartamentoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ObterDepartamentoQueryResult>> Handle(
        ObterDepartamentoQuery request, CancellationToken cancellationToken)
    {
        var d = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (d is null)
            return ResponseDefault<ObterDepartamentoQueryResult>.NotFound($"Departamento {request.Id} não encontrado.");

        return ResponseDefault<ObterDepartamentoQueryResult>.Ok(new ObterDepartamentoQueryResult(
            d.Id, d.Codigo, d.Nome, d.CentroDeCustoId, d.Ativo));
    }
}
