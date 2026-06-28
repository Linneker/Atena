using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ObterLotacao;

public sealed class ObterLotacaoQueryHandler
    : IRequestHandler<ObterLotacaoQuery, ResponseDefault<ObterLotacaoQueryResult>>
{
    private readonly ILotacaoRepository _repo;

    public ObterLotacaoQueryHandler(ILotacaoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ObterLotacaoQueryResult>> Handle(
        ObterLotacaoQuery request, CancellationToken cancellationToken)
    {
        var l = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (l is null)
            return ResponseDefault<ObterLotacaoQueryResult>.NotFound($"Lotação {request.Id} não encontrada.");

        return ResponseDefault<ObterLotacaoQueryResult>.Ok(new ObterLotacaoQueryResult(
            l.Id, l.Nome, l.EmpresaId, l.Cnpj, l.EnderecoJson, l.Ativo));
    }
}
