using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Despesa.Command.CriarDespesa;

public sealed record CriarDespesaCommandResult(Guid Id, string Nome, decimal Valor, DateTime DataVencimento);
