using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Divida.Command.CriarDivida;

public sealed record CriarDividaCommand(
    string Credor,
    string? Descricao,
    decimal ValorOriginal,
    decimal? TaxaJurosMensal,
    DateTime DataInicio,
    DateTime? DataFim,
    int NumeroParcelas) : IRequest<ResponseDefault<CriarDividaCommandResult>>;

public sealed record CriarDividaCommandResult(Guid Id, string Credor, decimal ValorOriginal);
