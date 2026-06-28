namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ObterLotacao;

public sealed record ObterLotacaoQueryResult(
    Guid Id,
    string Nome,
    Guid? EmpresaId,
    string? Cnpj,
    string? EnderecoJson,
    bool Ativo);
