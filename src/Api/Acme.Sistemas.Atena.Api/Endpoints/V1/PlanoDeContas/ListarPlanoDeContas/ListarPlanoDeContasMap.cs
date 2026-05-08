using Acme.Sistemas.Services.V1.PlanoDeContas.Query.ListarPlanoDeContas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas.ListarPlanoDeContas;

public static class ListarPlanoDeContasMap
{
    public static ListarPlanoDeContasQuery ToQuery(this ListarPlanoDeContasRequest _) => new();

    public static ListarPlanoDeContasResponse ToResponse(this ListarPlanoDeContasQueryResult result)
        => new(result.Raiz.Select(n => n.ToResponseNode()).ToArray());

    private static PlanoDeContasNoResponse ToResponseNode(this PlanoDeContasNode node)
        => new(node.Id, node.Codigo, node.Nome, node.Tipo, node.Nivel,
            node.AceitaLancamento, node.Ativo, node.PaiId,
            node.Filhos.Select(f => f.ToResponseNode()).ToList());
}
