using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Relatorios.Financeiro.Balanco;

public sealed record GerarBalancoQuery(DateTime DataReferencia)
    : IRequest<ResponseDefault<BalancoResult>>;

public sealed record BalancoLinha(string Descricao, decimal Valor);

public sealed record BalancoResult(
    DateTime DataReferencia,
    IReadOnlyList<BalancoLinha> Ativo,
    IReadOnlyList<BalancoLinha> Passivo,
    IReadOnlyList<BalancoLinha> PatrimonioLiquido,
    decimal TotalAtivo,
    decimal TotalPassivo,
    decimal TotalPatrimonioLiquido);
