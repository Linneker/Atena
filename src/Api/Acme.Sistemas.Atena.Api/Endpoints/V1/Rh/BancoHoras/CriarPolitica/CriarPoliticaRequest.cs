namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.CriarPolitica;

public sealed record CriarPoliticaRequest(
    string Nome,
    DateOnly VigenciaInicio,
    DateOnly? VigenciaFim,
    decimal LimiteHorasAcumular,
    int PrazoCompensacaoDias,
    bool PermitePagarExcedente,
    decimal FatorPagamento);
