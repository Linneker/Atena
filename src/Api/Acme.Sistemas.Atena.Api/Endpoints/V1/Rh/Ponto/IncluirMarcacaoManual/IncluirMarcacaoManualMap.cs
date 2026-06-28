using Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.IncluirMarcacaoManual;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.IncluirMarcacaoManual;

public static class IncluirMarcacaoManualMap
{
    public static IncluirMarcacaoManualCommand ToCommand(this IncluirMarcacaoManualRequest r)
        => new(r.FuncionarioId, r.DataHora, r.Tipo, r.Motivo);

    public static IncluirMarcacaoManualResponse ToResponse(this IncluirMarcacaoManualCommandResult r)
        => new(r.Id, r.HashIntegridade);
}
