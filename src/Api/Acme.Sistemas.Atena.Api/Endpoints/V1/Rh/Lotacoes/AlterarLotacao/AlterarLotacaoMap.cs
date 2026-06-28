using Acme.Sistemas.Services.V1.Rh.Lotacao.Command.AlterarLotacao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Lotacoes.AlterarLotacao;

public static class AlterarLotacaoMap
{
    public static AlterarLotacaoCommand ToCommand(this AlterarLotacaoRequest r)
        => new(r.Id, r.Nome, r.EmpresaId, r.Cnpj, r.EnderecoJson, r.Ativo);

    public static AlterarLotacaoResponse ToResponse(this AlterarLotacaoCommandResult r)
        => new(r.Id);
}
