namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cargos.ObterCargo;

public sealed record ObterCargoResponse(
    Guid Id,
    string? Codigo,
    string Descricao,
    string? CodigoCbo,
    decimal? SalarioBaseSugerido,
    bool Ativo);
