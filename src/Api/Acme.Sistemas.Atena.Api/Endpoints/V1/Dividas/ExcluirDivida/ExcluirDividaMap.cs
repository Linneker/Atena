using Acme.Sistemas.Services.V1.Divida.Command.ExcluirDivida;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.ExcluirDivida;

public static class ExcluirDividaMap
{
    public static ExcluirDividaCommand ToCommand(this ExcluirDividaRequest request)
        => new(request.Id);
}
