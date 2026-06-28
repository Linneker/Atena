using SrvCmd = Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.RejeitarSolicitacao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.RejeitarSolicitacao;

public static class RejeitarSolicitacaoMap
{
    public static SrvCmd.RejeitarSolicitacaoCommand ToCommand(this RejeitarSolicitacaoRequest request, Guid id)
        => new(id, request.Motivo);

    public static RejeitarSolicitacaoResponse ToResponse(this SrvCmd.RejeitarSolicitacaoCommandResult result)
        => new(result.Id, result.RejeitadoEm);
}
