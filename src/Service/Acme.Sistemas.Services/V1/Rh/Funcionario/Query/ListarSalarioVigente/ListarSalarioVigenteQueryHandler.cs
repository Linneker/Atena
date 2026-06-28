using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ListarSalarioVigente;

public sealed class ListarSalarioVigenteQueryHandler
    : IRequestHandler<ListarSalarioVigenteQuery, ResponseDefault<ListarSalarioVigenteQueryResult>>
{
    private readonly IHistoricoSalarioRepository _repo;

    public ListarSalarioVigenteQueryHandler(IHistoricoSalarioRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarSalarioVigenteQueryResult>> Handle(
        ListarSalarioVigenteQuery request, CancellationToken cancellationToken)
    {
        var h = await _repo.GetVigenteAsync(request.FuncionarioId, request.Em, cancellationToken);
        if (h is null)
            return ResponseDefault<ListarSalarioVigenteQueryResult>.Ok(
                new ListarSalarioVigenteQueryResult(null, null, null, null, null));

        return ResponseDefault<ListarSalarioVigenteQueryResult>.Ok(
            new ListarSalarioVigenteQueryResult(h.Id, h.Valor, h.VigenciaInicio, h.VigenciaFim, h.Motivo));
    }
}
