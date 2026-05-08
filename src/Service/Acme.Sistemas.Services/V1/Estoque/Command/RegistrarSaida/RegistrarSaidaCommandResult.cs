using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Services.V1.Estoque.Command.RegistrarSaida;

public sealed record RegistrarSaidaCommandResult(
    Guid MovimentoId, decimal NovoSaldoTotal, decimal NovoSaldoDisponivel);
