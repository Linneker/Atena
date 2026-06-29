namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ValidarRep;

public sealed record ValidacaoRepItemOutput(string Item, bool Ok, string? Mensagem);

public sealed record ValidarRepResponse(bool Apto, IReadOnlyList<ValidacaoRepItemOutput> Checagens);
