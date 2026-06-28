namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RegistrarSaida;

public sealed record RegistrarSaidaResponse(
    Guid MovimentoId,
    decimal NovoSaldoTotal,
    decimal NovoSaldoDisponivel);
