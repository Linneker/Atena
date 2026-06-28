using Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ObterLotacao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Lotacoes.ObterLotacao;

public static class ObterLotacaoMap
{
    public static ObterLotacaoQuery ToQuery(this ObterLotacaoRequest r) => new(r.Id);

    public static ObterLotacaoResponse ToResponse(this ObterLotacaoQueryResult r)
        => new(r.Id, r.Nome, r.EmpresaId, r.Cnpj, r.EnderecoJson, r.Ativo);
}
