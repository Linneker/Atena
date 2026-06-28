using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;
using Acme.Sistemas.Services.V1.Rh.Ponto.Espelho.Query.ObterEspelhoMensal;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ObterEspelhoMensal;

public static class ObterEspelhoMensalMap
{
    public static ObterEspelhoMensalQuery ToQuery(this ObterEspelhoMensalRequest r)
        => new(r.FuncionarioId, r.Competencia);

    public static ObterEspelhoMensalResponse ToResponse(this GeradorEspelhoMensal.EspelhoMensal e) => new(e);
}
