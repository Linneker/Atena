using Acme.Sistemas.Services.V1.CodigoServico.Query.ListarCodigosServico;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fiscal.CodigosServico.ListarCodigosServico;

public static class ListarCodigosServicoMap
{
    public static ListarCodigosServicoQuery ToQuery(this ListarCodigosServicoRequest _) => new();

    public static ListarCodigosServicoResponse ToResponse(this ListarCodigosServicoQueryResult result)
        => new(result.Items
            .Select(i => new ListarCodigosServicoResponseItem(i.Codigo, i.Descricao))
            .ToArray());
}
