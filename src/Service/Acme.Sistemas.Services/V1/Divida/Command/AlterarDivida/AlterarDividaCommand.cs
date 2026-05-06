using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Divida.Command.AlterarDivida;

public sealed record AlterarDividaCommand(
    Guid Id,
    string Credor,
    string? Descricao,
    decimal ValorOriginal,
    decimal? TaxaJurosMensal,
    DateTime DataInicio,
    DateTime? DataFim,
    int NumeroParcelas) : IRequest<ResponseDefault<AlterarDividaCommandResult>>;

public sealed record AlterarDividaCommandResult(Guid Id);
