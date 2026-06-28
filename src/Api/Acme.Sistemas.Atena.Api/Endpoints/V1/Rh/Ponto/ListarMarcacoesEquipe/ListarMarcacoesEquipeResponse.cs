using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarMarcacoesEquipe;

public sealed record ListarMarcacoesEquipeResponseItem(
    Guid Id, DateTime DataHora, TipoMarcacao Tipo, OrigemMarcacao Origem, StatusMarcacao Status);

public sealed record ListarMarcacoesEquipeResponse(
    IReadOnlyList<ListarMarcacoesEquipeResponseItem> Items, int Total);
