using Acme.Sistemas.Domain.Reports;
using Acme.Sistemas.Services.V1.Relatorios.Financeiro.Balanco;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Balanco.GerarBalanco;

public static class GerarBalancoMap
{
    public static GerarBalancoQuery ToQuery(this GerarBalancoRequest request)
        => new(request.DataReferencia);

    public static GerarBalancoResponse ToResponse(this BalancoResult result)
        => new(result.DataReferencia,
            result.Ativo.Select(l => l.ToResponseLinha()).ToArray(),
            result.Passivo.Select(l => l.ToResponseLinha()).ToArray(),
            result.PatrimonioLiquido.Select(l => l.ToResponseLinha()).ToArray(),
            result.TotalAtivo, result.TotalPassivo, result.TotalPatrimonioLiquido);

    private static BalancoLinhaResponse ToResponseLinha(this BalancoLinha linha)
        => new(linha.Descricao, linha.Valor);
}
