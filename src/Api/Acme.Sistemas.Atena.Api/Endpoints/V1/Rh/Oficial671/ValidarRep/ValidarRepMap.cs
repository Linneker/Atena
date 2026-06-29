using Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ValidarRep;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ValidarRep;

public static class ValidarRepMap
{
    public static ValidarRepQuery ToQuery(this ValidarRepRequest r) => new(r.EmpresaId);

    public static ValidarRepResponse ToResponse(this ValidarRepQueryResult r)
        => new(r.Apto, r.Checagens
            .Select(c => new ValidacaoRepItemOutput(c.Item, c.Ok, c.Mensagem)).ToList());
}
