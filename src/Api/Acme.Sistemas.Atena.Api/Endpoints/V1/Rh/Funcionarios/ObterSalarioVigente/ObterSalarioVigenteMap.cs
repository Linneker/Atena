using Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ListarSalarioVigente;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.ObterSalarioVigente;

public static class ObterSalarioVigenteMap
{
    public static ListarSalarioVigenteQuery ToQuery(this ObterSalarioVigenteRequest r)
        => new(r.FuncionarioId, r.Em);

    public static ObterSalarioVigenteResponse ToResponse(this ListarSalarioVigenteQueryResult r)
        => new(r.HistoricoSalarioId, r.Valor, r.VigenciaInicio, r.VigenciaFim, r.Motivo);
}
