using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Query.ListarCentrosDeCusto;

public sealed record ListarCentrosDeCustoQuery(int Skip = 0, int Take = 100)
    : IRequest<ResponseDefault<ListarCentrosDeCustoQueryResult>>;

public sealed record ListarCentrosDeCustoQueryItem(
    Guid Id, string Codigo, string Nome, string? Descricao,
    Guid? ResponsavelId, bool Ativo);

public sealed record ListarCentrosDeCustoQueryResult(IReadOnlyList<ListarCentrosDeCustoQueryItem> Items);
