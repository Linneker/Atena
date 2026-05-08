using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Divida.Command.CriarDivida;

public sealed record CriarDividaCommandResult(Guid Id, string Credor, decimal ValorOriginal);
