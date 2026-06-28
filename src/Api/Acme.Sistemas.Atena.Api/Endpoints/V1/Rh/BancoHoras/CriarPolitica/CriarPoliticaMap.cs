using Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.CriarPolitica;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.CriarPolitica;

public static class CriarPoliticaMap
{
    public static CriarPoliticaCommand ToCommand(this CriarPoliticaRequest r)
        => new(r.Nome, r.VigenciaInicio, r.VigenciaFim, r.LimiteHorasAcumular,
               r.PrazoCompensacaoDias, r.PermitePagarExcedente, r.FatorPagamento);

    public static CriarPoliticaResponse ToResponse(this CriarPoliticaCommandResult r)
        => new(r.Id, r.Nome);
}
