using Acme.Sistemas.Services.V1.Relatorios.Financeiro.DRE;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Dre.GerarDrePdf;

public static class GerarDrePdfMap
{
    public static GerarDREQuery ToQuery(this GerarDrePdfRequest request)
        => new(request.Inicio, request.Fim);
}
