using Acme.Sistemas.Services.V1.Rh.Cargo.Query.ObterCargo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cargos.ObterCargo;

public static class ObterCargoMap
{
    public static ObterCargoQuery ToQuery(this ObterCargoRequest r) => new(r.Id);

    public static ObterCargoResponse ToResponse(this ObterCargoQueryResult r)
        => new(r.Id, r.Codigo, r.Descricao, r.CodigoCbo, r.SalarioBaseSugerido, r.Ativo);
}
