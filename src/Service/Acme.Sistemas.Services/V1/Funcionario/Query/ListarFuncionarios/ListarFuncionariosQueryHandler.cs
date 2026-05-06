using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Funcionario.Query.ListarFuncionarios;

public sealed class ListarFuncionariosQueryHandler
    : IRequestHandler<ListarFuncionariosQuery, ResponseDefault<ListarFuncionariosQueryResult>>
{
    private readonly IFuncionarioRepository _repo;

    public ListarFuncionariosQueryHandler(IFuncionarioRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarFuncionariosQueryResult>> Handle(ListarFuncionariosQuery request, CancellationToken cancellationToken)
    {
        var funcs = await _repo.ListAsync(request.Skip, request.Take, cancellationToken);
        var items = funcs.Select(f => new ListarFuncionariosQueryItem(
            f.Id, f.NomeCompleto, f.Cpf, f.Email,
            f.Cargo, f.Departamento, f.CentroDeCustoId,
            f.DataAdmissao, f.DataDemissao, f.Status)).ToList();
        return ResponseDefault<ListarFuncionariosQueryResult>.Ok(
            new ListarFuncionariosQueryResult(items));
    }
}
