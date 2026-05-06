using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Query.ListarPlanoDeContas;

public sealed record ListarPlanoDeContasQuery() : IRequest<ResponseDefault<ListarPlanoDeContasQueryResult>>;

public sealed record PlanoDeContasNode(
    Guid Id, string Codigo, string Nome, TipoConta Tipo,
    int Nivel, bool AceitaLancamento, bool Ativo,
    Guid? PaiId, IList<PlanoDeContasNode> Filhos);

public sealed record ListarPlanoDeContasQueryResult(IReadOnlyList<PlanoDeContasNode> Raiz);
