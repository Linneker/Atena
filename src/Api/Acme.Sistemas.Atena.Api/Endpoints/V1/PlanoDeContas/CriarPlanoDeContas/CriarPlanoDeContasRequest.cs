using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas.CriarPlanoDeContas;

public sealed record CriarPlanoDeContasRequest(
    string Codigo,
    string Nome,
    TipoConta Tipo,
    Guid? PaiId,
    bool AceitaLancamento);
