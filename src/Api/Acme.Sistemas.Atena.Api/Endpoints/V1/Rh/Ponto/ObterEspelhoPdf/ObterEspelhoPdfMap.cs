using Acme.Sistemas.Services.V1.Rh.Ponto.Espelho.Query.ObterEspelhoMensal;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ObterEspelhoPdf;

public static class ObterEspelhoPdfMap
{
    public static ObterEspelhoMensalQuery ToQuery(this ObterEspelhoPdfRequest r)
        => new(r.FuncionarioId, r.Competencia);
}
