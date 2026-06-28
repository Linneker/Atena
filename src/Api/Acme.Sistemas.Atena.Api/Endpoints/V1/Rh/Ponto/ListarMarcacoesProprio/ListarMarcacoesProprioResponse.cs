using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarMarcacoesProprio;

public sealed record ListarMarcacoesProprioResponseItem(
    Guid Id, DateTime DataHora, TipoMarcacao Tipo, OrigemMarcacao Origem,
    StatusMarcacao Status, string HashIntegridade);

public sealed record ListarMarcacoesProprioResponse(
    IReadOnlyList<ListarMarcacoesProprioResponseItem> Items, int Total);
