using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Services.V1.Estoque.Command.RegistrarEntrada;

public sealed record RegistrarEntradaCommandResult(
    Guid MovimentoId, decimal NovoSaldoTotal, decimal NovoSaldoDisponivel);
