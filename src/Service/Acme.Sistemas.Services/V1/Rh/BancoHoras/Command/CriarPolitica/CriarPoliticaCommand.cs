using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.CriarPolitica;

public sealed record CriarPoliticaCommand(
    string Nome,
    DateOnly VigenciaInicio,
    DateOnly? VigenciaFim,
    decimal LimiteHorasAcumular,
    int PrazoCompensacaoDias,
    bool PermitePagarExcedente,
    decimal FatorPagamento) : IRequest<ResponseDefault<CriarPoliticaCommandResult>>;
