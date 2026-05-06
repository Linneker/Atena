using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Fornecedor.Query.ListarFornecedores;

public sealed class ListarFornecedoresQueryHandler
    : IRequestHandler<ListarFornecedoresQuery, ResponseDefault<ListarFornecedoresQueryResult>>
{
    private readonly IFornecedorRepository _repo;

    public ListarFornecedoresQueryHandler(IFornecedorRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarFornecedoresQueryResult>> Handle(ListarFornecedoresQuery request, CancellationToken cancellationToken)
    {
        var fornecedores = await _repo.ListByFiltroAsync(request.Termo, request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountByFiltroAsync(request.Termo, cancellationToken);

        var items = fornecedores.Select(f => new ListarFornecedoresQueryItem(
            f.Id, f.Tipo, f.Nome, f.NomeFantasia, f.Documento,
            f.Email, f.Telefone, f.CondicaoPagamentoPadrao, f.Status)).ToList();

        return ResponseDefault<ListarFornecedoresQueryResult>.Ok(
            new ListarFornecedoresQueryResult(items, total));
    }
}
