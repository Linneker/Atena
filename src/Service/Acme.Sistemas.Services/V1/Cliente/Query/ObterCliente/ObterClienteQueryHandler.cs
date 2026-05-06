using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Cliente.Query.ObterCliente;

public sealed class ObterClienteQueryHandler
    : IRequestHandler<ObterClienteQuery, ResponseDefault<ObterClienteQueryResult>>
{
    private readonly IClienteRepository _repo;

    public ObterClienteQueryHandler(IClienteRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ObterClienteQueryResult>> Handle(ObterClienteQuery request, CancellationToken cancellationToken)
    {
        var c = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (c is null)
            return ResponseDefault<ObterClienteQueryResult>.NotFound("Cliente não encontrado.");

        return ResponseDefault<ObterClienteQueryResult>.Ok(new ObterClienteQueryResult(
            c.Id, c.Tipo, c.Nome, c.NomeFantasia,
            c.Documento, c.InscricaoEstadual,
            c.Email, c.Telefone,
            c.Status, c.Inadimplente, c.BloqueadoVendas,
            c.Endereco, c.CreatedAt));
    }
}
