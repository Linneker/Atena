using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ObterBeneficioCatalogo;

public sealed class ObterBeneficioCatalogoQueryHandler
    : IRequestHandler<ObterBeneficioCatalogoQuery, ResponseDefault<ObterBeneficioCatalogoQueryResult>>
{
    private readonly IBeneficioCatalogoRepository _repo;

    public ObterBeneficioCatalogoQueryHandler(IBeneficioCatalogoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ObterBeneficioCatalogoQueryResult>> Handle(
        ObterBeneficioCatalogoQuery request, CancellationToken cancellationToken)
    {
        var b = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (b is null)
            return ResponseDefault<ObterBeneficioCatalogoQueryResult>.NotFound(
                $"Benefício {request.Id} não encontrado.");

        return ResponseDefault<ObterBeneficioCatalogoQueryResult>.Ok(new ObterBeneficioCatalogoQueryResult(
            b.Id, b.Codigo, b.Descricao, b.Tipo,
            b.DescontoFuncionarioPct, b.CustoEmpresaPadrao,
            b.NaturezaRubricaEsocial, b.Ativo));
    }
}
