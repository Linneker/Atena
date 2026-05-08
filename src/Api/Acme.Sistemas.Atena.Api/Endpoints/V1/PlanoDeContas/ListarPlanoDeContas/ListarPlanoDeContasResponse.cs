using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.PlanoDeContas.ListarPlanoDeContas;

public sealed record PlanoDeContasNoResponse(
    Guid Id,
    string Codigo,
    string Nome,
    TipoConta Tipo,
    int Nivel,
    bool AceitaLancamento,
    bool Ativo,
    Guid? PaiId,
    IList<PlanoDeContasNoResponse> Filhos);

public sealed record ListarPlanoDeContasResponse(IReadOnlyList<PlanoDeContasNoResponse> Raiz);
