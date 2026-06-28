namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RegistrarEntrada;

public sealed record RegistrarEntradaResponse(
    Guid MovimentoId,
    decimal NovoSaldoTotal,
    decimal NovoSaldoDisponivel);
