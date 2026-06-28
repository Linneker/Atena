using Acme.Sistemas.Services.V1.Rh.Lotacao.Command.RemoverLotacao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Lotacoes.RemoverLotacao;

public static class RemoverLotacaoMap
{
    public static RemoverLotacaoCommand ToCommand(this RemoverLotacaoRequest r) => new(r.Id);
    public static RemoverLotacaoResponse ToResponse(this RemoverLotacaoCommandResult r) => new(r.Id);
}
