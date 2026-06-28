using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Funcionario.Query.ListarFuncionarios;

public sealed class ListarFuncionariosQueryHandler
    : IRequestHandler<ListarFuncionariosQuery, ResponseDefault<ListarFuncionariosQueryResult>>
{
    private readonly IFuncionarioRepository _repo;
    private readonly ICentroDeCustoRepository _centros;

    public ListarFuncionariosQueryHandler(IFuncionarioRepository repo, ICentroDeCustoRepository centros)
    {
        _repo = repo;
        _centros = centros;
    }

    public async Task<ResponseDefault<ListarFuncionariosQueryResult>> Handle(ListarFuncionariosQuery request, CancellationToken cancellationToken)
    {
        var funcs = await _repo.ListAsync(request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountAsync(cancellationToken);

        var centroIds = funcs.Where(f => f.CentroDeCustoId.HasValue).Select(f => f.CentroDeCustoId!.Value);
        var nomesCentro = await _centros.GetNomesByIdsAsync(centroIds, cancellationToken);

        var items = funcs.Select(f => new ListarFuncionariosQueryItem(
            f.Id, f.NomeCompleto, f.Cpf, f.Email,
            f.Cargo, f.Departamento,
            f.CentroDeCustoId,
            f.CentroDeCustoId.HasValue && nomesCentro.TryGetValue(f.CentroDeCustoId.Value, out var nome) ? nome : null,
            f.DataAdmissao, f.DataDemissao, f.Status)).ToList();
        return ResponseDefault<ListarFuncionariosQueryResult>.Ok(
            new ListarFuncionariosQueryResult(items, total));
    }
}
