namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas.AlterarPlanoDeContas;

public sealed record AlterarPlanoDeContasRequest(string Nome, bool AceitaLancamento, bool Ativo);
