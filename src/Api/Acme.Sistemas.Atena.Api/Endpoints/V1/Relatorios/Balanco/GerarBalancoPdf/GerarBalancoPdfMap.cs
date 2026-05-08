using Acme.Sistemas.Services.V1.Relatorios.Financeiro.Balanco;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Balanco.GerarBalancoPdf;

public static class GerarBalancoPdfMap
{
    public static GerarBalancoQuery ToQuery(this GerarBalancoPdfRequest request)
        => new(request.DataReferencia);
}
