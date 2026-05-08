namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas.CriarPlanoDeContas;

public sealed record CriarPlanoDeContasResponse(Guid Id, string Codigo, string Nome, int Nivel);
