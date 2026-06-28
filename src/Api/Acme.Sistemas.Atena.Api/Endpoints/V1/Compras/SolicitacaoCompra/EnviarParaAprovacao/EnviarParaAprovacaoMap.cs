using SrvCmd = Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.EnviarParaAprovacao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.EnviarParaAprovacao;

public static class EnviarParaAprovacaoMap
{
    public static SrvCmd.EnviarParaAprovacaoCommand ToCommand(this EnviarParaAprovacaoRequest request)
        => new(request.Id);

    public static EnviarParaAprovacaoResponse ToResponse(this SrvCmd.EnviarParaAprovacaoCommandResult result)
        => new(result.Id, result.ValorTotal, result.PermissaoNecessaria);
}
