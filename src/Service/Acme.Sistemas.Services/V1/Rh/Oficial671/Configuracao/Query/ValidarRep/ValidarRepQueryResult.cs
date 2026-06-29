namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ValidarRep;

public sealed record ValidarRepQueryResult(
    bool Apto,
    IReadOnlyList<ValidacaoRepItem> Checagens);

public sealed record ValidacaoRepItem(string Item, bool Ok, string? Mensagem);
