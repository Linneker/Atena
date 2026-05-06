using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Query.ListarPlanoDeContas;

public sealed class ListarPlanoDeContasQueryHandler
    : IRequestHandler<ListarPlanoDeContasQuery, ResponseDefault<ListarPlanoDeContasQueryResult>>
{
    private readonly IPlanoDeContasRepository _repo;

    public ListarPlanoDeContasQueryHandler(IPlanoDeContasRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarPlanoDeContasQueryResult>> Handle(ListarPlanoDeContasQuery request, CancellationToken cancellationToken)
    {
        var todas = await _repo.ListAllAsync(cancellationToken);

        var nodes = todas.ToDictionary(
            c => c.Id,
            c => new PlanoDeContasNode(
                c.Id, c.Codigo, c.Nome, c.Tipo, c.Nivel,
                c.Aceita_Lancamento, c.Ativo, c.PaiId,
                new List<PlanoDeContasNode>()));

        var raiz = new List<PlanoDeContasNode>();
        foreach (var c in todas)
        {
            var node = nodes[c.Id];
            if (c.PaiId.HasValue && nodes.TryGetValue(c.PaiId.Value, out var pai))
                pai.Filhos.Add(node);
            else
                raiz.Add(node);
        }

        return ResponseDefault<ListarPlanoDeContasQueryResult>.Ok(
            new ListarPlanoDeContasQueryResult(raiz));
    }
}
