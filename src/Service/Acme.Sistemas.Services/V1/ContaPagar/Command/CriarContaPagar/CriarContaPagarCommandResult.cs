using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ContaPagar.Command.CriarContaPagar;

public sealed record CriarContaPagarCommandResult(Guid Id, string Descricao, decimal ValorOriginal, DateTime DataVencimento);
