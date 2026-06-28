using SrvCmd = Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.AprovarSolicitacao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.AprovarSolicitacao;

public static class AprovarSolicitacaoMap
{
    public static SrvCmd.AprovarSolicitacaoCommand ToCommand(this AprovarSolicitacaoRequest request)
        => new(request.Id);

    public static AprovarSolicitacaoResponse ToResponse(this SrvCmd.AprovarSolicitacaoCommandResult result)
        => new(result.Id, result.AprovadoEm);
}
