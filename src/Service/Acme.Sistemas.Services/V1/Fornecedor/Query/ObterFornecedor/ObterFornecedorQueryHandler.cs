using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Fornecedor.Query.ObterFornecedor;

public sealed class ObterFornecedorQueryHandler
    : IRequestHandler<ObterFornecedorQuery, ResponseDefault<ObterFornecedorQueryResult>>
{
    private readonly IFornecedorRepository _repo;

    public ObterFornecedorQueryHandler(IFornecedorRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ObterFornecedorQueryResult>> Handle(ObterFornecedorQuery request, CancellationToken cancellationToken)
    {
        var f = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (f is null)
            return ResponseDefault<ObterFornecedorQueryResult>.NotFound("Fornecedor não encontrado.");

        return ResponseDefault<ObterFornecedorQueryResult>.Ok(new ObterFornecedorQueryResult(
            f.Id, f.Tipo, f.Nome, f.NomeFantasia,
            f.Documento, f.InscricaoEstadual,
            f.Email, f.Telefone,
            f.CondicaoPagamentoPadrao, f.Status,
            f.Endereco, f.CreatedAt));
    }
}
