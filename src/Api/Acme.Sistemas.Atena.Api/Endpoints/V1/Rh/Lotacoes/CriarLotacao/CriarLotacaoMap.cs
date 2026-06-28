using Acme.Sistemas.Services.V1.Rh.Lotacao.Command.CriarLotacao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Lotacoes.CriarLotacao;

public static class CriarLotacaoMap
{
    public static CriarLotacaoCommand ToCommand(this CriarLotacaoRequest r)
        => new(r.Nome, r.EmpresaId, r.Cnpj, r.EnderecoJson);

    public static CriarLotacaoResponse ToResponse(this CriarLotacaoCommandResult r)
        => new(r.Id, r.Nome);
}
