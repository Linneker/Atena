using Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Query.ObterComprovantePdf;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ObterComprovantePdf;

public static class ObterComprovantePdfMap
{
    public static ObterComprovantePdfQuery ToQuery(this ObterComprovantePdfRequest r)
        => new(r.MarcacaoId);
}
