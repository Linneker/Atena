namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cargos.CriarCargo;

public sealed record CriarCargoRequest(
    string? Codigo,
    string Descricao,
    string? CodigoCbo,
    decimal? SalarioBaseSugerido);
