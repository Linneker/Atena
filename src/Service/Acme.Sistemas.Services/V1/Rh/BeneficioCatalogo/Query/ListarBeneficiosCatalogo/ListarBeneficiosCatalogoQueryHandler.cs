using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ListarBeneficiosCatalogo;

public sealed class ListarBeneficiosCatalogoQueryHandler
    : IRequestHandler<ListarBeneficiosCatalogoQuery, ResponseDefault<ListarBeneficiosCatalogoQueryResult>>
{
    private readonly IBeneficioCatalogoRepository _repo;

    public ListarBeneficiosCatalogoQueryHandler(IBeneficioCatalogoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarBeneficiosCatalogoQueryResult>> Handle(
        ListarBeneficiosCatalogoQuery request, CancellationToken cancellationToken)
    {
        var benefs = await _repo.ListAsync(request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountAsync(cancellationToken);

        var items = benefs
            .Select(b => new ListarBeneficiosCatalogoQueryItem(
                b.Id, b.Codigo, b.Descricao, b.Tipo,
                b.DescontoFuncionarioPct, b.CustoEmpresaPadrao, b.Ativo))
            .ToList();

        return ResponseDefault<ListarBeneficiosCatalogoQueryResult>.Ok(
            new ListarBeneficiosCatalogoQueryResult(items, total));
    }
}
