using Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ObterFichaCompleta;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.ObterFichaCompleta;

public static class ObterFichaCompletaMap
{
    public static ObterFichaCompletaQuery ToQuery(this ObterFichaCompletaRequest r) => new(r.FuncionarioId);

    public static ObterFichaCompletaResponse ToResponse(this ObterFichaCompletaQueryResult r) => new(r);
}
