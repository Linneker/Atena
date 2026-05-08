using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Command.CriarReceita;

public sealed record CriarReceitaCommandResult(Guid Id, string Nome, decimal Valor, DateTime DataPrevistaRecebimento);
