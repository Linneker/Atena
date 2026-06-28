using Acme.Sistemas.Services.V1.ConciliacaoBancaria.Command.ImportarExtrato;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ConciliacaoBancaria.ImportarExtrato;

public static class ImportarExtratoMap
{
    public static ImportarExtratoCommand ToCommand(this ImportarExtratoRequest request)
        => new(request.Banco, request.Agencia, request.Conta, request.Formato, request.Arquivo);

    public static ImportarExtratoResponse ToResponse(this ImportarExtratoCommandResult result)
        => new(result.ConciliacaoId, result.TotalLancamentos, result.TotalConciliados);
}
