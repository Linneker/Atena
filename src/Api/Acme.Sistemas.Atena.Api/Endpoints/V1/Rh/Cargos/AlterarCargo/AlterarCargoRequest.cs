namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cargos.AlterarCargo;

public sealed record AlterarCargoRequest(
    Guid Id,
    string? Codigo,
    string Descricao,
    string? CodigoCbo,
    decimal? SalarioBaseSugerido,
    bool Ativo);
