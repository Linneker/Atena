using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ObterEspelhoMensal;

// Wrapper imutável que envelopa o Result do engine para serialização JSON.
public sealed record ObterEspelhoMensalResponse(GeradorEspelhoMensal.EspelhoMensal Espelho);
